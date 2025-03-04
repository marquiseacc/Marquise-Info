document.addEventListener('DOMContentLoaded', function () {
    const modals = document.querySelectorAll('.modal');


    modals.forEach(function (modal) {
        const videoScript = modal.querySelector('script[id="videoScript"]'); 
        const iframe = modal.querySelector('iframe'); 


        $(modal).on('hidden.bs.modal', function () {

            if (iframe) {
                const iframeSrc = iframe.src;
                iframe.src = ''; 
                iframe.src = iframeSrc; 
            }

            if (videoScript) {
                videoScript.remove(); 
            }
        });

        const closeButton = modal.querySelector('.closes');
        if (closeButton) {
            closeButton.addEventListener('click', function () {

               
                if (iframe) {
                    const iframeSrc = iframe.src;
                    iframe.src = ''; 
                    iframe.src = iframeSrc; 
                }

                if (videoScript) {
                    videoScript.remove(); 
                }
            });
        }
    });
});
