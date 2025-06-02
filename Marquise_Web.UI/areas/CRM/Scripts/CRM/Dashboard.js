// توابع کمکی جلالی خارج از event
function toJalali(gy, gm, gd) {
    const g_d_m = [0, 31, (gy % 4 === 0 && gy % 100 !== 0 || gy % 400 === 0) ? 29 : 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
    let gy2 = (gm > 2) ? (gy + 1) : gy;
    let days = 355666 + (365 * gy) + Math.floor((gy2 + 3) / 4) - Math.floor((gy2 + 99) / 100) + Math.floor((gy2 + 399) / 400) + gd;
    for (let i = 0; i < gm; ++i) days += g_d_m[i];
    let jy = -1595 + 33 * Math.floor(days / 12053); days %= 12053;
    jy += 4 * Math.floor(days / 1461); days %= 1461;
    if (days > 365) { jy += Math.floor((days - 1) / 365); days = (days - 1) % 365; }
    const jm_list = [0, 31, 31, 31, 31, 31, 31, 30, 30, 30, 30, 30, 29];
    let jm = 0;
    for (let i = 1; i <= 12 && days >= jm_list[i]; ++i) {
        days -= jm_list[i];
        jm = i;
    }
    const jd = days + 1;
    return { jy, jm: jm + 1, jd };
}

function isLeapYear(year) {
    return [1, 5, 9, 13, 17, 22, 26, 30].includes(year % 33);
}

function getMonthLength(jy, jm) {
    if (jm <= 6) return 31;
    if (jm <= 11) return 30;
    return isLeapYear(jy) ? 30 : 29;
}

function generateDaysOfYear(jy) {
    const days = [];
    for (let jm = 1; jm <= 12; jm++) {
        const monthLength = getMonthLength(jy, jm);
        for (let jd = 1; jd <= monthLength; jd++) {
            days.push({
                date: `${jy}-${String(jm).padStart(2, '0')}-${String(jd).padStart(2, '0')}`
            });
        }
    }
    return days;
}

function getCurrentJalaliYear() {
    const now = new Date();
    const { jy } = toJalali(now.getFullYear(), now.getMonth() + 1, now.getDate());
    return jy;
}

function getTodayJalaliDate() {
    const now = new Date();
    const j = toJalali(now.getFullYear(), now.getMonth() + 1, now.getDate());
    return `${j.jy}-${String(j.jm).padStart(2, '0')}-${String(j.jd).padStart(2, '0')}`;
}

document.addEventListener('DOMContentLoaded', function () {
    const globalLoader = document.getElementById("global-loading");
    if (globalLoader) globalLoader.style.display = "none";

    if (!window.location.pathname.startsWith('/CRM/Dashboard')) return;

    const token = localStorage.getItem("jwtToken");
    if (!token) {
        alert("لطفاً وارد شوید.");
        window.location.href = "/CRM/Auth/SendOtp";
        return;
    }

    // بارگذاری MainDetail در صورت وجود
    const mainDetail = document.querySelector('#main-detail');
    if (mainDetail) {
        fetchWithLoading('/CRM/Dashboard/MainDetail', {
            method: 'GET',
            headers: { 'Authorization': 'Bearer ' + token }
        }, '#main-detail').catch(e => console.error('❌ خطا در بارگذاری جزئیات:', e));
    }

    // بارگذاری LastTicket در صورت وجود
    const lastTicket = document.querySelector('#last-ticket');
    if (lastTicket) {
        fetchWithLoading('/CRM/Dashboard/LastTicket', {
            method: 'GET',
            headers: { 'Authorization': 'Bearer ' + token }
        }, '#last-ticket').catch(e => console.error('❌ خطا در بارگذاری آخرین تیکت:', e));
    }

    const timeline = document.getElementById('timeline');
    if (timeline) {
        fetch('/api/CRM/DashboardApi/SupportTimeLine', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            }
        })
            .then(response => {
                if (response.status === 401) {
                    alert("دسترسی ندارید، لطفاً دوباره وارد شوید.");
                    window.location.href = "/CRM/Auth/SendOtp";
                    return;
                }
                if (!response.ok) throw new Error(`خطا: ${response.status}`);
                return response.json();
            })
            .then(result => {
                if (result && result.success) {
                    const activeRanges = result.data;
                    const currentYear = getCurrentJalaliYear();
                    const todayDate = getTodayJalaliDate();
                    const days = generateDaysOfYear(currentYear);
                    timeline.innerHTML = '';

                    days.forEach(({ date }, index) => {
                        const dayDiv = document.createElement('div');
                        dayDiv.classList.add('day');

                        if (index === 0) dayDiv.classList.add('first');
                        if (index === days.length - 1) dayDiv.classList.add('last');

                        let isActive = false;
                        let isStart = false;
                        let isEnd = false;

                        activeRanges.forEach(range => {
                            if (date === range.StartDate) {
                                isActive = true;
                                isStart = true;
                            } else if (date === range.EndDate) {
                                isActive = true;
                                isEnd = true;
                            } else if (date > range.StartDate && date < range.EndDate) {
                                isActive = true;
                            }
                        });

                        if (isActive) {
                            dayDiv.classList.add('active');
                            if (isStart) dayDiv.classList.add('start');
                            if (isEnd) dayDiv.classList.add('end');
                        }

                        if (date === todayDate) {
                            dayDiv.classList.add('today');
                            dayDiv.title = date + ' (امروز)';
                        } else {
                            dayDiv.title = date;
                        }

                        timeline.appendChild(dayDiv);
                    });
                } else {
                    console.error("❌ دریافت اطلاعات تایم‌لاین ناموفق بود");
                }
            })
            .catch(error => {
                console.error("❌ خطا در بارگذاری تایم‌لاین:", error);
            });
    }
});
