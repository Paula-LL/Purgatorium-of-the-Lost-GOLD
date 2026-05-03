// ============================================================
//  ATLAS TRIGGER FUNCTION: processarSessio
//  Projecte: Purgatorium of the Lost - GOLD
// ============================================================
//  CONFIGURACIÓ DEL TRIGGER A ATLAS APP SERVICES:
//    - Tipus:         Database Trigger
//    - Nom:           onNovaSessioJoc
//    - Cluster:       (el teu cluster)
//    - Database:      PurgatoriumDB
//    - Collection:    sessions_joc
//    - Operation:     Insert
//    - Full Document: Activat (✓)
//    - Function:      processarSessio  ← aquest fitxer
// ============================================================

exports = async function(changeEvent) {

  // 1. Obtenir el document inserit des de Unity
  const sessio = changeEvent.fullDocument;

  if (!sessio || !sessio.playerId) {
    console.log("Sessió invàlida: falta playerId");
    return;
  }

  const playerId = sessio.playerId;
  const stats    = sessio.estadistiques || {};

  // 2. Accedir a la col·lecció d'estadístiques agregades
  const mongodb    = context.services.get("Cluster0");  // Nom del cluster a Atlas App Services
  const colEstats  = mongodb.db("PurgatoriumDB").collection("estadistiques_jugador");
  const colSessions = mongodb.db("PurgatoriumDB").collection("sessions_joc");

  try {
    // 3. Actualitzar estadístiques agregades del jugador (upsert)
    //    $inc: acumular valors numèrics de forma atòmica
    //    $max: guardar el valor màxim (per exemple, la sessió més llarga)
    //    $set: actualitzar timestamp de l'última sessió
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
          // Només s'estableix quan es crea el document per primer cop
          primeraPartida: sessio.iniciSessio || new Date()
        }
      },
      { upsert: true }
    );

    // 4. Calcular ràtio de victòries i actualitzar-lo
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
