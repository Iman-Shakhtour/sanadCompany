document.addEventListener("DOMContentLoaded", () => {
  fetch("http://localhost:5261/api/partners")
    .then(res => res.json())
    .then(partners => {
      const container = document.getElementById("carouselInner");
      container.innerHTML = "";

      if (!partners.length) {
        container.innerHTML = "<div class='text-white text-center'>لا يوجد شركاء حالياً.</div>";
        return;
      }

      partners.forEach((partner, index) => {
        const isActive = index === 0 ? "active" : "";
        const item = document.createElement("div");
        item.className = `carousel-item ${isActive}`;

        const row = document.createElement("div");
        row.className = "d-flex justify-content-center align-items-center gap-5 py-5";

        const logoSrc = partner.logoUrl
          ? `http://localhost:5261/PartnerLogos/${partner.logoUrl}`
          : "../assets/img/default-logo.png";

        for (let i = 0; i < 3; i++) {
          const logoBox = document.createElement("div");
          logoBox.innerHTML = `
            <img src="${logoSrc}" alt="${partner.name}" class="img-fluid" style="max-width: 100px;" />
          `;
          row.appendChild(logoBox);
        }

        item.appendChild(row);
        container.appendChild(item);
      });
    })
    .catch(err => {
      console.error("خطأ عند تحميل الشركاء:", err);
      document.getElementById("carouselInner").innerHTML = "<p class='text-danger'>فشل في تحميل الشركاء.</p>";
    });
});