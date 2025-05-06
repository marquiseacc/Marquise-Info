document.addEventListener('DOMContentLoaded', function () {
    const activeRanges = [
        { from: '1404-01-10', to: '1404-03-15' },
        { from: '1404-06-01', to: '1404-09-10' },
        { from: '1404-10-20', to: '1404-12-28' }
    ];

   

    // بررسی کبیسه بودن سال شمسی
    function isLeapYear(year) {
        return (year % 33 === 1 || year % 33 === 5 || year % 33 === 9 || year % 33 === 13 || year % 33 === 17 || year % 33 === 22 || year % 33 === 26 || year % 33 === 30);
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

    const days = generateDaysOfYear(1404);
    const timeline = document.getElementById('timeline');

    days.forEach(({ date }, index) => {
        const dayDiv = document.createElement('div');
        dayDiv.classList.add('day');

        // اگر اولین یا آخرین روز کل سال
        if (index === 0) dayDiv.classList.add('first');
        if (index === days.length - 1) dayDiv.classList.add('last');

        let isActive = false;
        let isStart = false;
        let isEnd = false;

        activeRanges.forEach(range => {
            if (date === range.from) {
                isActive = true;
                isStart = true;
            } else if (date === range.to) {
                isActive = true;
                isEnd = true;
            } else if (date > range.from && date < range.to) {
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

});