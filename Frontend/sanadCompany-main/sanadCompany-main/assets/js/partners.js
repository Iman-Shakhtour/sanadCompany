document.addEventListener("DOMContentLoaded", () => {
    fetch("http://localhost:5261/api/partners")
        .then(res => res.json())
        .then(partners => {
            const grid = document.getElementById("partnersGrid");
            grid.innerHTML = "";
            partners.forEach(partner => {
                const logoSrc = partner.logoUrl
                    ? `http://localhost:5261/PartnerLogos/${partner.logoUrl}`
                    : "assets/img/default-logo.png";
                const partnerHtml = `
                    <div class="partner-card">
                        <img src="${logoSrc}" alt="${partner.name}" class="partner-logo" />
                        <h4>${partner.name}</h4>
                        ${partner.website ? `<a href="${partner.website}" target="_blank">الموقع</a>` : ""}
                    </div>
                `;
                grid.insertAdjacentHTML("beforeend", partnerHtml);
            });
        })
        .catch(err => {
            document.getElementById("partnersGrid").innerHTML = "<p>فشل في تحميل الشركاء.</p>";
        });
});