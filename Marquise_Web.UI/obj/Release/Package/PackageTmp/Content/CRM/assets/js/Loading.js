//document.addEventListener('DOMContentLoaded', function () {
//    const globalLoader = document.getElementById("global-loading");
    

//    let completedCount = 0;

//    function hideGlobalLoaderWhenDone() {
//        completedCount++;
//        if (completedCount === partialsToLoad.length) {
//            if (globalLoader) globalLoader.style.display = "none";
//        }
//    }

//    function showInlineLoader(targetId) {
//        const target = document.getElementById(targetId);
//        if (target) {
//            target.innerHTML = `
//            <div class="sk-cube-grid">
//              <div class="sk-cube sk-cube1"></div>
//              <div class="sk-cube sk-cube2"></div>
//              <div class="sk-cube sk-cube3"></div>
//              <div class="sk-cube sk-cube4"></div>
//              <div class="sk-cube sk-cube5"></div>
//              <div class="sk-cube sk-cube6"></div>
//              <div class="sk-cube sk-cube7"></div>
//              <div class="sk-cube sk-cube8"></div>
//              <div class="sk-cube sk-cube9"></div>
//            </div>`;
//        }
//    }

//    function loadPartial(url, targetId) {
//        showInlineLoader(targetId);
//        fetch(url)
//            .then(response => {
//                if (!response.ok) throw new Error("Network response was not ok");
//                return response.text();
//            })
//            .then(html => {
//                const target = document.getElementById(targetId);
//                if (target) target.innerHTML = html;
//            })
//            .catch(error => {
//                const target = document.getElementById(targetId);
//                if (target) target.innerHTML = "خطا در بارگذاری.";
//                console.error(error);
//            })
//            .finally(() => {
//                hideGlobalLoaderWhenDone(); // همیشه صدا زده بشه چه موفق چه خطا
//            });
//    }

//    // شروع نمایش لودر کلی
//    if (globalLoader) globalLoader.style.display = "block";

    
//});
