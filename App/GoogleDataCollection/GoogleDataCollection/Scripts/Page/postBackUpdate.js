var totalServices;
var uploadedServices;
var uploadCompleted;
$(document).ready(function () {

    // Get the instance of PageRequestManager.
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    // Add initializeRequest and endRequest
    prm.add_initializeRequest(prm_InitializeRequest);
    prm.add_endRequest(prm_EndRequest);

    // Called when async postback begins
    function prm_InitializeRequest(sender, args) {
        // initial loading display (if needed). 
        if (!totalServices && !uploadCompleted) {
            totalServices = serviceDetails.length+1;
            uploadedServices = 0;
            
        }

        $('#infoDisplay').val(uploadedServices + "/" + totalServices);

    }

    // Called when async postback ends
    function prm_EndRequest(sender, args) {
        if (!uploadCompleted) {
            uploadedServices++;
            $('#infoDisplay').val(uploadedServices + "/" + totalServices);

            // either upload the next service or go to the next step
            if (serviceDetails.length > 0) {
                updateServiceData();
            } else {
                totalServices = false;
                uploadedServices = false;
                uploadCompleted = true;
                uploadReturn();
            }
        }
    }
})

