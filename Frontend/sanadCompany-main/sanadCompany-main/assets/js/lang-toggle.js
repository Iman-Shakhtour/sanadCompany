// // assets/lang-toggle.js
// document.addEventListener("DOMContentLoaded", function () {
//   const langToggle = document.getElementById("langToggle");
//   const path = window.location.pathname;

//   // تحديد اللغة الحالية من الرابط
//   const currentLang = path.includes("/en/") ? "en" : "ar";
//   const newLang = currentLang === "en" ? "ar" : "en";

//   // تحديث نص الكبسة
//   langToggle.textContent = currentLang === "en" ? "العربية" : "English";

//   langToggle.addEventListener("click", () => {
//     // تحويل المسار للغة الجديدة
//     const newPath = path.replace(`/${currentLang}/`, `/${newLang}/`);
//     window.location.href = newPath;
//   });
// });

// // assets/lang-toggle.js to small screen
// document.addEventListener("DOMContentLoaded", function () {
//   const langToggle = document.getElementById("langToggle1");
//   const path = window.location.pathname;

//   // تحديد اللغة الحالية من الرابط
//   const currentLang = path.includes("/en/") ? "en" : "ar";
//   const newLang = currentLang === "en" ? "ar" : "en";

//   // تحديث نص الكبسة
//   langToggle.textContent = currentLang === "en" ? "العربية" : "English";

//   langToggle.addEventListener("click", () => {
//     // تحويل المسار للغة الجديدة
//     const newPath = path.replace(`/${currentLang}/`, `/${newLang}/`);
//     window.location.href = newPath;
//   });
// });

// assets/lang-toggle.js
// assets/lang-toggle.js
// document.addEventListener("DOMContentLoaded", function () {
//   function setupLangToggle(toggleId) {
//     const langToggle = document.getElementById(toggleId);
//     if (!langToggle) return;

//     const path = window.location.pathname;

//     // تحديد اللغة الحالية من خلال المسار
//     const isArabic = path.includes("/ar/");
//     const currentLang = isArabic ? "ar" : "en";
//     const newLang = isArabic ? "en" : "ar";

//     // تحديث نص الزر
//     langToggle.textContent = isArabic ? "English" : "العربية";

//     langToggle.addEventListener("click", () => {
//       let newPath;

//       if (currentLang === "ar") {
//         // من العربية إلى الإنجليزية
//         newPath = "/index.html";
//       } else {
//         // من الإنجليزية إلى العربية
//         newPath = "/ar/index.html";
//       }

//       window.location.href = newPath;
//     });
//   }

//   // دعم لكل من الأزرار في الشاشات المختلفة
//   setupLangToggle("langToggle");
//   setupLangToggle("langToggle1");
// });
// assets/lang-toggle.js
document.addEventListener("DOMContentLoaded", function () {
  function setupLangToggle(toggleId) {
    const langToggle = document.getElementById(toggleId);
    if (!langToggle) return;

    const path = window.location.pathname;

    const isArabic = path.includes("/ar/");
    const currentLang = isArabic ? "ar" : "en";
    const currentPage = path.substring(path.lastIndexOf("/") + 1) || "index.html";

    langToggle.textContent = isArabic ? "English" : "العربية";

    langToggle.addEventListener("click", () => {
      let newPath;

      if (currentLang === "ar") {
        // من العربي للإنجليزي
        newPath = `../${currentPage}`;
      } else {
        // من الإنجليزي للعربي
        newPath = `ar/${currentPage}`;
      }

      window.location.href = newPath;
    });
  }

  setupLangToggle("langToggle");
  setupLangToggle("langToggle1");
});
