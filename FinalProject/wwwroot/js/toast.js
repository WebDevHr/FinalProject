    $(document).ready(function () {
        if ('@TempData["success"]' !== '') {
            toastr.options.closeButton = true;
            toastr.options.closeEasing = 'swing';
            toastr.success('@TempData["success"]');
        }
    });
