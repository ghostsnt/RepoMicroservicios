// estadoInactividad.js

export function iniciarControlEstado(userId, updateEndpoint) {
    let estadoActual = "online";
    let inactividadTimer = null;
    const segundosAusente = 60;     // 1 minuto para marcar como "ausente"
    const segundosOffline = 120;    // 2 minutos totales para marcar como "offline"

    function actualizarEstado(nuevoEstado) {
        if (estadoActual === nuevoEstado) return;
        estadoActual = nuevoEstado;

        fetch(updateEndpoint, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                idUsuario: parseInt(userId),
                estado: nuevoEstado,
                estaEnLinea: nuevoEstado !== "offline",
                ultimaConexion: new Date().toISOString()
            })
        }).then(response => {
            if (!response.ok) {
                console.error("❌ Error al enviar estado:", response.status);
            } else {
                console.log(`✅ Estado actualizado a "${nuevoEstado}"`);
            }
        });
    }

    function iniciarTemporizadorInactividad() {
        if (inactividadTimer) clearTimeout(inactividadTimer);

        inactividadTimer = setTimeout(() => {
            if (estadoActual === "online") {
                actualizarEstado("ausente");
                iniciarTemporizadorInactividad();
            } else if (estadoActual === "ausente") {
                actualizarEstado("offline");
            }
        }, estadoActual === "online" ? segundosAusente * 1000 : segundosOffline * 1000);
    }

    function reiniciarTemporizador() {
        if (estadoActual !== "online") {
            actualizarEstado("online");
        }
        iniciarTemporizadorInactividad();
    }

    // Detectar cualquier actividad
    ["mousemove", "keydown", "mousedown", "scroll"].forEach(evt =>
        document.addEventListener(evt, reiniciarTemporizador)
    );

    // Comenzar por defecto como "online"
    actualizarEstado("online");
    iniciarTemporizadorInactividad();
}
