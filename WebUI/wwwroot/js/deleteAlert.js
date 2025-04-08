
function confirmDelete(controller, action, id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "Do you want to delete this?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete',
        cancelButtonText: 'Cancel'
    }).then((result) => {
        if (result.isConfirmed) {
            let url = `/${controller}/${action}/${id}`;
            url = url.replace(/\/+/g, '/');
            window.location.href = url;
        }
    });
}