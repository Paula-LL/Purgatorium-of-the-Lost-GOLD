exports = async function(changeEvent) {

  const sessio = changeEvent.fullDocument;

  if (!sessio || !sessio.playerId) {
    console.log("Sessió invàlida: falta playerId");
    return;
  }

  const playerId = sessio.playerId;
  const stats    = sessio.estadistiques || {};

  const mongodb    = context.services.get("Cluster0");
  const colEstats  = mongodb.db("PurgatoriumDB").collection("estadistiques_jugador");
  const colSessions = mongodb.db("PurgatoriumDB").collection("sessions_joc");

  try {
    await colEstats.updateOne(
      { playerId: playerId },
      {
        $inc: {
          totalPartidesJugades:  stats.partidesJugades  || 0,
          totalVictorias:        stats.victorias         || 0,
          totalEnemicsElimitats: stats.enemicsElimitats  || 0,
          totalBossesDerrots:    stats.bossesDerrots     || 0,
          totalMorts:            stats.morts             || 0,
          totalDanyHet:          stats.danyHet           || 0,
          totalDanyRebut:        stats.danyRebut         || 0,
          totalCartesRecollides: stats.cartesRecollides  || 0,
          totalTempsJugat:       stats.tempsTotal        || 0,
          totalSessions:         1
        },
        $max: {
          sessioMesLlarga: sessio.duradaSessioSeg || 0
        },
        $set: {
          ultimaSessio:  sessio.fiSessio || new Date(),
          plataforma:    sessio.plataforma || "unknown",
          versioJoc:     sessio.versioJoc  || "unknown"
        },
        $setOnInsert: {
          primeraPartida: sessio.iniciSessio || new Date()
        }
      },
      { upsert: true }
    );

    const estatActual = await colEstats.findOne({ playerId: playerId });
    if (estatActual && estatActual.totalPartidesJugades > 0) {
      const ratio = estatActual.totalVictorias / estatActual.totalPartidesJugades;
      await colEstats.updateOne(
        { playerId: playerId },
        { $set: { ratioVictorias: parseFloat(ratio.toFixed(2)) } }
      );
    }

    console.log(`[Trigger] Estadístiques actualitzades per: ${playerId}`);

  } catch (err) {
    console.error(`[Trigger] Error processant sessió per ${playerId}: ${err.message}`);
  }
};
