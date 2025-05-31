document.addEventListener("DOMContentLoaded", function () {
    
    const trigger = document.getElementById("branch-menu-trigger");
    const container = document.getElementById("branch-menu-container");

    if (!trigger || !container) {
        console.error("❌ trigger یا container یافت نشد.");
        return;
    }

    const token = localStorage.getItem("jwtToken");

    if (!token) {
        console.error("❌ توکن یافت نشد.");
        return;
    }

    // بارگذاری لیست شعبه‌ها با توکن
    fetch("/Base/BranchMenu", {
        method: "GET",
        headers: {
            "Authorization": "Bearer " + token,
            "X-Requested-With": "XMLHttpRequest"
        }
    })
        .then(response => {
            if (response.status === 401 || response.status === 403) {
                console.error(`❌ خطای احراز هویت (${response.status}): دسترسی غیرمجاز.`);
                container.innerHTML = '<li class="dropdown-item text-danger">احراز هویت انجام نشد</li>';
                return null;
            }

            if (!response.ok) {
                console.error(`❌ خطای ناشناخته (${response.status})`);
                container.innerHTML = '<li class="dropdown-item text-danger">خطا در دریافت اطلاعات شعب</li>';
                return null;
            }

            return response.json();
        })
        .then(data => {
            if (!data) return;

            container.innerHTML = "";

            if (!Array.isArray(data) || data.length === 0) {
                container.innerHTML = '<li class="dropdown-item text-muted">شعبه‌ای یافت نشد</li>';
                return;
            }

            data.forEach(branch => {
                const li = document.createElement("li");
                li.className = "dropdown-item font-13 cursor-pointer";

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
                            'Authorization': `Bearer ${token}`
                        },
                        body: JSON.stringify(accountVM)
                    })
                        .then(res => res.json())
                        .then(data => {
                            console.log("Response from SetClaims:", data);
                            if (data.token) {
                                localStorage.setItem('jwtToken', data.token);
                                window.location.href = '/CRM/Dashboard/Index';
                            } else {
                                Swal.fire('خطا', 'توکن دریافت نشد.', 'error');
                            }
                        })
                        .catch(err => {
                            console.error(err);
                            Swal.fire('خطا', 'خطا در ارتباط با سرور', 'error');
                        });
                });

                container.appendChild(li);
            });

            // Divider
            const divider = document.createElement("li");
            divider.className = "dropdown-divider";
            container.appendChild(divider);

            // Logout option
            const logout = document.createElement("li");
            logout.className = "dropdown-item font-13";
            logout.innerHTML = `
    <a href="#" onclick="handleLogoutClick(event)">
        <i class="icon-power ml-2 font-15"></i> خروج
    </a>`;
            container.appendChild(logout);

        })
        .catch(error => {
            console.error("❌ خطا در دریافت داده:", error);
            container.innerHTML = '<li class="dropdown-item text-danger">خطا در دریافت اطلاعات شعب</li>';
        });
});

function handleLogoutClick(event) {
    event.preventDefault(); // جلوگیری از رفتار پیش‌فرض لینک

    localStorage.removeItem("jwtToken"); // حذف توکن از localStorage

    window.location.href = "/CRM/Auth/Logout"; // هدایت به اکشن لاگ‌اوت سرور
}
