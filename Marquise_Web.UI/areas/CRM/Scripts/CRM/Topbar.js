document.addEventListener("DOMContentLoaded", function () {
    const trigger = document.getElementById("branch-menu-trigger");
    const container = document.getElementById("branch-menu-container");

    if (!trigger || !container) {
        console.error("trigger یا container یافت نشد.");
        return;
    }

    let loaded = false;  // برای اطمینان، هرچند لازم نیست چون فقط یکبار بارگذاری میکنیم

    // بارگذاری لیست شعبه ها در ابتدای لود صفحه
    fetch("/Base/BranchMenu", {
        headers: { "X-Requested-With": "XMLHttpRequest" }
    })
        .then(response => response.json())
        .then(data => {
            container.innerHTML = "";

            if (!Array.isArray(data) || data.length === 0) {
                container.innerHTML = '<li class="dropdown-item text-muted">شعبه‌ای یافت نشد</li>';
                return;
            }

            data.forEach(branch => {
                const li = document.createElement("li");
                li.className = "dropdown-item font-13 cursor-pointer";

                // ذخیره داده‌ها در data attributes
                li.dataset.accountId = branch.AccountId;
                li.dataset.userId = branch.UserId || "";
                li.dataset.crmAccountId = branch.CrmAccountId || "";
                li.dataset.name = branch.Name;

                li.innerHTML = `<i class="bi bi-building ml-2 font-15"></i> ${branch.Name}`;

                li.addEventListener("click", function () {
                    const accountVM = {
                        AccountId: this.dataset.accountId,
                        UserId: this.dataset.userId,
                        CrmAccountId: this.dataset.crmAccountId,
                        Name: this.dataset.name
                    };

                    fetch('/CRM/Auth/SetClaims', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json',
                            'X-Requested-With': 'XMLHttpRequest'
                        },
                        body: JSON.stringify(accountVM)
                    })
                        .then(resp => {
                            if (resp.ok) {
                                window.location.reload();
                            } else {
                                alert("خطا در ارسال اطلاعات به سرور.");
                            }
                        })
                        .catch(err => {
                            console.error("خطا در ارسال درخواست:", err);
                            alert("خطا در ارسال اطلاعات به سرور.");
                        });
                });

                container.appendChild(li);
            });

            const divider = document.createElement("li");
            divider.className = "dropdown-divider";
            container.appendChild(divider);

            const logout = document.createElement("li");
            logout.className = "dropdown-item font-13";
            logout.innerHTML = `
            <a href="/CRM/Auth/Logout">
                <i class="icon-power ml-2 font-15"></i> خروج
            </a>`;
            container.appendChild(logout);

            loaded = true;  // بارگذاری انجام شده
        })
        .catch(error => {
            console.error("خطا در دریافت داده:", error);
            container.innerHTML = '<li class="dropdown-item text-danger">خطا در دریافت اطلاعات شعب</li>';
        });

});
