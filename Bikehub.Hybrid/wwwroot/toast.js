// Add this configuration before calling any toastr methods
toastr.options = {
    "progressBar": true,       // This enables the progress bar
    "positionClass": "toast-top-right",
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",         // How long the toast stays (ms)
    "extendedTimeOut": "1000", // How long it stays after hovering
    "closeButton": true        // Optional: adds an 'X' button
};

window.showToastr = (type, message) => {
    if (type === "success") toastr.success(message);
    if (type === "error") toastr.error(message);
    if (type === "info") toastr.info(message);
    if (type === "warning") toastr.warning(message);
}

window.showToastrAdvanced = (type, message, showProgress) => {
    toastr.options.progressBar = showProgress;
    toastr[type](message);
}