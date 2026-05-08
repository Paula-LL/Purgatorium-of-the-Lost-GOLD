exports = async function(changeEvent) {

  const sessio = changeEvent.fullDocument;

  if (!sessio || !sessio.playerId) {
    console.log("[analitzarProgres] Sessió invàlida: falta playerId");
    return;
  }

  const playerId = sessio.playerId;
  const stats    = sessio.estadistiques || {};

  const enemics    = stats.enemicsElimitats || 0;
  const partides   = stats.partidesJugades  || 1;
  const morts      = stats.morts            || 0;
  const bosses     = stats.bossesDerrots    || 0;
  const durada     = sessio.duradaSessioSeg || 0;

  const eficienciaCombat = danyRebut > 0
    ? parseFloat((danyHet / danyRebut).toFixed(2))
    : danyHet > 0 ? 99.99 : 0;

  const mitjanaEnemicsPerPartida = parseFloat((enemics / partides).toFixed(2));

  let nivelDificultat;
  if (morts === 0)       nivelDificultat = "facil";
  else if (morts <= 2)   nivelDificultat = "mig";
  else                   nivelDificultat = "dificil";

  const sessioSenseMorts = morts === 0 ? 1 : 0;

  const tempsMitjaPerBoss = bosses > 0
    ? parseFloat((durada / bosses).toFixed(1))
    : 0;

  const mongodb    = context.services.get("Cluster0");
  const colProgres = mongodb.db("PurgatoriumDB").collection("progres_jugador");

  try {
    await colProgres.updateOne(
      { playerId: playerId },
      {
        $inc: {
          sumaEficienciaCombat:       eficienciaCombat,
          sumaMitjanaEnemics:         mitjanaEnemicsPerPartida,
          totalSessiosSenseMorts:     sessioSenseMorts,
          sumaTempsMitjaPerBoss:      tempsMitjaPerBoss,
          totalSessionsAnalitzades:   1
        },
        $push: {
          historiNivelDificultat: nivelDificultat
        },
        $set: {
          ultimNivelDificultat:   nivelDificultat,
          ultimaSessio:           sessio.fiSessio || new Date()
        },
        $setOnInsert: {
          primeraSessionAnalitzada: sessio.iniciSessio || new Date()
        }
      },
      { upsert: true }
    );

    const docProgres = await colProgres.findOne({ playerId: playerId });
    if (docProgres && docProgres.totalSessionsAnalitzades > 0) {
      const n = docProgres.totalSessionsAnalitzades;
      await colProgres.updateOne(
        { playerId: playerId },
        {
          $set: {
            eficienciaCombatMitjana:     parseFloat((docProgres.sumaEficienciaCombat / n).toFixed(2)),
            mitjanaEnemicsGlobal:        parseFloat((docProgres.sumaMitjanaEnemics   / n).toFixed(2)),
            tempsMitjaPerBossGlobal:     parseFloat((docProgres.sumaTempsMitjaPerBoss / n).toFixed(1)),
            ratioSessiosSenseMorts:      parseFloat((docProgres.totalSessiosSenseMorts / n).toFixed(2))
          }
        }
      );
    }

    console.log(`[analitzarProgres] Progrés actualitzat per: ${playerId}`);

  } catch (err) {
    console.error(`[analitzarProgres] Error processant sessió per ${playerId}: ${err.message}`);
  }
};
