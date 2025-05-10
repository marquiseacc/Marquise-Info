document.addEventListener('DOMContentLoaded', function () {
    fetch("/CRM/Dashboard/MainDetail")
        .then(response => {
            if (!response.ok) throw new Error("Network response was not ok");
            return response.text();
        })
        .then(html => {
            document.getElementById("main-detail").innerHTML = html;
        })
        .catch(error => {
            document.getElementById("main-detail").innerHTML = "خطا در بارگذاری .";
        });


    fetch("/CRM/Dashboard/LastTicket")
        .then(response => {
            if (!response.ok) throw new Error("Network response was not ok");
            return response.text();
        })
        .then(html => {
            document.getElementById("last-ticket").innerHTML = html;
        })
        .catch(error => {
            console.log(error);
            document.getElementById("last-ticket").innerHTML = "خطا در بارگذاری .";
        });



    const currentYear = getCurrentJalaliYear();

    fetch('/api/CRM/DashboardApi/SupportTimeLine', {
        method: 'GET',
        headers: { 'Content-Type': 'application/json' }
    })
        .then(response => response.json())
        .then(result => {
            if (result.success) {
                const activeRanges = result.data;

                const days = generateDaysOfYear(currentYear);
                const timeline = document.getElementById('timeline');
                const dayWidth = 100 / days.length;
                timeline.innerHTML = ''; // پاک‌سازی قبلی

                days.forEach(({ date }, index) => {
                    const dayDiv = document.createElement('div');
                    dayDiv.classList.add('day');
                    dayDiv.style.width = `${dayWidth}%`;

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

                    dayDiv.title = date;
                    timeline.appendChild(dayDiv);
                });

            } else {
                console.error("دریافت اطلاعات ناموفق بود");
            }
        });


    // توابع عمومی
    // دریافت سال شمسی جاری از تاریخ میلادی
    function getCurrentJalaliYear() {
        const now = new Date();
        const gYear = now.getFullYear();
        const gMonth = now.getMonth() + 1;
        const gDay = now.getDate();

        // تبدیل میلادی به شمسی
        const g2j = toJalali(gYear, gMonth, gDay);
        return g2j.jy;
    }

    // تبدیل تاریخ میلادی به شمسی
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

    // بررسی کبیسه بودن سال شمسی
    function isLeapYear(year) {
        return [1, 5, 9, 13, 17, 22, 26, 30].includes(year % 33);
    }

    // گرفتن تعداد روزهای هر ماه شمسی
    function getMonthLength(jy, jm) {
        if (jm <= 6) return 31;
        if (jm <= 11) return 30;
        return isLeapYear(jy) ? 30 : 29;
    }

    // تولید لیست کل روزهای سال شمسی
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

});