(() => {
  // حدد اللغة حسب ما يخزّنه المستخدم (أو إفتراضياً "en")
  const lang = window.APP_LANG || localStorage.getItem("lang") || "en";
  const BASE = "http://localhost:5261/api/products";

  document.addEventListener("DOMContentLoaded", () => {
    loadProducts(`${BASE}?lang=${lang}`,            "all-products");
    loadProducts(`${BASE}?year=2023&lang=${lang}`, "products-2023");
    loadProducts(`${BASE}?year=2024&lang=${lang}`, "products-2024");
    loadProducts(`${BASE}?category=ai&lang=${lang}`,     "products-ai");
    loadProducts(`${BASE}?category=health&lang=${lang}`, "products-health");
  });

  function loadProducts(endpoint, containerId) {
    fetch(endpoint)
      .then(res => res.json())
      .then(products => {
        const container = document.getElementById(containerId);
        container.innerHTML = "";

        if (!products.length) {
          // نص مختلف بناءً على اللغة
          container.innerHTML = `<div class="text-${lang==='ar'?'white':'dark'}">
            ${lang==='ar'?'لا توجد منتجات':'No products found'}.
          </div>`;
          return;
        }

        products.forEach(product => {
          const card = createProductCard(product);
          container.appendChild(card);
        });
      })
      .catch(err => console.error("Error fetching products:", err));
  }

  function createProductCard(product) {
    return htmlToElement(`
      <div class="col-md-4 product-card">
        <div class="card h-100 bg-dark text-white">
          <img src="http://localhost:5261/ProductImages/${product.imageUrl}"
               class="card-img-top" alt="${product.title}">
          <div class="card-body d-flex flex-column">
            <h5 class="card-title">${product.title}</h5>
            <p class="card-text">${product.description}</p>
            <a href="prodacts.html" class="btn btn-outline-light mt-auto">
              ${lang==='ar'?'اقرأ المزيد':'Read More'}
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
