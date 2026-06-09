document.addEventListener("DOMContentLoaded", () => {

    Sortable.create(document.getElementById('columnasContainer'), {
        animation: 150,
        handle: '.header-columna',

        onEnd: function () {

            const ids = [...document.querySelectorAll('.draggable-columna')]
                .map(el => parseInt(el.dataset.id));

            
            fetch("/Columna/Reordenar", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    tableroId: TABLERO_ID,
                    ids: ids
                })
            });
        }
    });

    document.querySelectorAll('.tareas-container').forEach(container => {
        Sortable.create(container, {
            group: "tareas",   
            animation: 150,
            draggable: ".tarea",
            easing: "cubic-bezier(0.2, 1, 0.1, 1)",

            onEnd: function (evt) {

                const newColId = evt.to.dataset.columnaid;
                const oldColId = evt.from.dataset.columnaid;

                const idsNew = [...evt.to.querySelectorAll('.tarea')]
                    .map(t => parseInt(t.dataset.id))
                    .filter(Boolean);

                const idsOld = [...evt.from.querySelectorAll('.tarea')]
                    .map(t => parseInt(t.dataset.id))
                    .filter(Boolean);

                fetch("/Tarea/Reordenar", {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify({
                        columnaOrigenId: oldColId,
                        columnaDestinoId: newColId,
                        idsOrigen: idsOld,
                        idsDestino: idsNew
                    })
                });
            }
        });
    });
});
