let formularioAbierto = null;

function mostrarInput(boton) {

    const form = boton.closest('form');
    const contenedor = form.querySelector('.contenedor-input');

    
    cerrarInput();

    contenedor.innerHTML = `
    <div class="nueva-tarea bg-white p-3 rounded-lg shadow border border-slate-200 space-y-3">

        <input
            type="text"
            name="titulo"
            placeholder="Título de la tarjeta..."
            class="w-full rounded-md border border-slate-300 p-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
        />

        <div class="flex gap-2">
            <button
                type="submit"
                class="bg-indigo-600 text-white px-4 py-2 rounded-md text-sm hover:bg-indigo-700">
                Añadir
            </button>

            <button
                type="button"
                class="bg-slate-200 text-slate-700 px-4 py-2 rounded-md text-sm hover:bg-slate-300"
                onclick="cerrarInput()">
                Cancelar
            </button>
        </div>

    </div>
    `;

    formularioAbierto = contenedor;

    const input = contenedor.querySelector('input');
    input.focus();

    setTimeout(() => {
        document.addEventListener('click', detectarClickFuera);
    }, 0);
}

function cerrarInput() {

    if (formularioAbierto) {
        formularioAbierto.innerHTML = '';
        formularioAbierto = null;
    }

    document.removeEventListener('click', detectarClickFuera);
}

function detectarClickFuera(event) {

    if (!formularioAbierto) return;

    const tarjeta = formularioAbierto.querySelector('.nueva-tarea');

    if (!tarjeta.contains(event.target) &&
        !event.target.closest('button[onclick^="mostrarInput"]')) {
        cerrarInput();
    }
}

async function cambiarEstado(tareaId, tableroId, btn) {

    const formData = new URLSearchParams();
    formData.append("tareaId", tareaId);
    formData.append("tableroId", tableroId);

    const response = await fetch('/Tarea/CambiarEstadoTarea', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        body: formData
    });

    if (response.ok) {
        
        btn.classList.toggle("bg-green-500");
        btn.classList.toggle("border-green-500");
        btn.classList.toggle("bg-transparent");
        btn.classList.toggle("border-slate-300");
    }
}