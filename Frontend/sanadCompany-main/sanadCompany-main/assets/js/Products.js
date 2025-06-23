document.addEventListener("DOMContentLoaded", () => {
  fetch("http://localhost:5261/api/products?lang=en")
    .then(response => response.json())
    .then(products => {
      const container = document.getElementById("productsContainer");
      container.innerHTML = "";

      products.forEach(product => {
        const card = document.createElement("div");
        card.className = "col-md-4";

        card.innerHTML = `
          <div class="card mb-4 shadow-sm">
            <img src="http://localhost:5261/Images/${product.imageUrl}" class="card-img-top" alt="${product.title}">
            <div class="card-body text-center">
              <h5 class="card-title">${product.title}</h5>
              <p class="card-text">${product.description}</p>
              <div class="d-flex justify-content-center gap-2 mt-3">
                <a href="${product.buyLink || '#'}" class="btn btn-primary btn-sm">Buy Now</a>
                <a href="${product.detailsLink}" class="btn btn-outline-primary btn-sm">Details</a>
              </div>
            </div>
          </div>
        `;
        container.appendChild(card);
      });
    })
    .catch(error => {
      console.error("Failed to fetch products:", error);
    });
});
