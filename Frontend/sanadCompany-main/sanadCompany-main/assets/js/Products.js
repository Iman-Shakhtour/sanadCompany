(() => {
  // حدد اللغة حسب ما يخزّنه المستخدم (أو إفتراضياً "en")
  const lang = window.APP_LANG || localStorage.getItem("lang") || "en";
  const BASE = "http://localhost:5261/api/products";

    // loadProducts(`${BASE}?lang=${lang}`,            "all-products");
    // loadProducts(`${BASE}?year=2023&lang=${lang}`, "products-2023");
    // loadProducts(`${BASE}?year=2024&lang=${lang}`, "products-2024");
    // loadProducts(`${BASE}?category=ai&lang=${lang}`,     "products-ai");
    // loadProducts(`${BASE}?category=health&lang=${lang}`, "products-health");
    document.addEventListener("DOMContentLoaded", () => {
  const tabsContainer = document.getElementById("productTabs");
  const contentContainer = document.getElementById("productTabsContent");

  // أول تبويب: "الكل"
  const allTabId = "all-products";
  tabsContainer.innerHTML = `
      <li class="nav-item">
      <button class="nav-link active text-secondary" type="button" onclick="filterProductsTab('${allTabId}', this)">
        ${lang === "ar" ? "الكل" : "All"}
      </button>
    </li>
  `;
contentContainer.innerHTML = `
    <div class="tab-pane fade show active" id="${allTabId}" role="tabpanel">
      <div class="row g-4" id="${allTabId}-container"></div>
    </div>
  `;




  // جلب التصنيفات من الـ API
  fetch(`http://localhost:5261/api/categories?lang=${lang}`)
    .then(res => res.json())
    .then(categories => {
      categories.forEach(cat => {
        const tabId = `cat-${cat.code}`;
        const containerId = `${tabId}-container`;

        // أضف زر التبويب
        tabsContainer.innerHTML += `
          <li class="nav-item">
            <button class="nav-link text-secondary" type="button" onclick="filterProductsTab('${tabId}', this)">
              ${cat.name}
            </button>
          </li>
        `;

        // أضف محتوى التبويب
        contentContainer.innerHTML += `
          <div class="tab-pane fade" id="${tabId}" role="tabpanel">
            <div class="row g-4" id="${containerId}"></div>
          </div>
        `;

        // حمّل المنتجات لهذا التصنيف
        loadProducts(`${BASE}?category=${cat.code}&lang=${lang}`, containerId);
      });

      // أخيرًا حمّل الكل
      loadProducts(`${BASE}?lang=${lang}`, `${allTabId}-container`);
    })
    .catch(err => console.error("Error loading categories:", err));
});

  

  function loadProducts(endpoint, containerId) {
  fetch(endpoint)
    .then(res => res.json())
    .then(products => {
      const container = document.getElementById(containerId);
      container.innerHTML = "";

      console.log("🚀 Loading into:", containerId);
      console.log("📦 Number of products:", products.length);
      console.log("👀 Container element:", container);

      if (!products.length) {
        container.innerHTML = `<div class="text-${lang==='ar'?'white':'dark'}">
          ${lang==='ar'?'لا توجد منتجات':'No products found'}.
        </div>`;
        return;
      }

      products.forEach(product => {
        const card = createProductCard(product);
        console.log("🧱 Card element:", card);
        container.appendChild(card);
      });
    })
    .catch(err => console.error("Error fetching products:", err));
}


 function createProductCard(product) {
  return htmlToElement(`
    <div class="col-4">
      <div class="card h-100 bg-dark text-white">
        <img src="http://localhost:5261/ProductImages/${product.imageUrl}"
             class="card-img-top" alt="${product.title}">
        <div class="card-body d-flex flex-column">
          <h5 class="card-title">${product.title}</h5>
          <p class="card-text">${product.description}</p>
          <a href="prodacts.html" class="btn btn-outline-light mt-auto">
            ${lang === 'ar' ? 'اقرأ المزيد' : 'Read More'}
          </a>
        </div>
      </div>
    </div>
  `);
}


  // تحويل نص HTML إلى عنصر DOM
  function htmlToElement(html) {
    const template = document.createElement('template');
    template.innerHTML = html.trim();
    return template.content.firstChild;
  }
})();

window.filterProductsTab = function(tabId, btn) {
  document.querySelectorAll('.tab-pane').forEach(p => {
    p.classList.remove('show', 'active');
  });
  document.querySelectorAll('.nav-link').forEach(b => {
    b.classList.remove('active');
  });

  document.getElementById(tabId).classList.add('show', 'active');
  btn.classList.add('active');
};
