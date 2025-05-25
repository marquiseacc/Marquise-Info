window.addEventListener("load", function () {
    const globalLoader = document.getElementById("global-loading");
    if (globalLoader) globalLoader.style.display = "none";
});

document.addEventListener('DOMContentLoaded', function () {

    function showInlineLoader(targetId) {
        document.getElementById(targetId).innerHTML = `
        <div class="sk-cube-grid">
          <div class="sk-cube sk-cube1"></div>
          <div class="sk-cube sk-cube2"></div>
          <div class="sk-cube sk-cube3"></div>
          <div class="sk-cube sk-cube4"></div>
          <div class="sk-cube sk-cube5"></div>
          <div class="sk-cube sk-cube6"></div>
          <div class="sk-cube sk-cube7"></div>
          <div class="sk-cube sk-cube8"></div>
          <div class="sk-cube sk-cube9"></div>
        </div>`;
    }

    function loadPartial(url, targetId) {
        showInlineLoader(targetId);
        fetch(url)
            .then(response => {
                if (!response.ok) throw new Error("Network response was not ok");
                return response.text();
            })
            .then(html => {
                document.getElementById(targetId).innerHTML = html;
            })
            .catch(error => {
                document.getElementById(targetId).innerHTML = "خطا در بارگذاری.";
                console.error(error);
            });
    }

    // فراخوانی دیتا با لودر موضعی
    loadPartial("/CRM/Dashboard/MainDetail", "main-detail");
    loadPartial("/CRM/Dashboard/LastTicket", "last-ticket");

});