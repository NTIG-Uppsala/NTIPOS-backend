const addProductButton = document.getElementById("add-product-btn")
const addProductForm = document.getElementById("add-product-form")

const allProducts = []

document.addEventListener("DOMContentLoaded", () => {
    fetchData()
    waitForData()
})

addProductForm.onsubmit = function() {
    var formData = new FormData(document.getElementById("add-product-form"))
    formData = Object.fromEntries(formData)
    formData.stock = formData.stock == "" ? 0 : formData.stock
    console.log(fetch("/api/v1/products", {method: "POST", body: JSON.stringify(formData), headers: {"Content-Type": "application/json",}}))
    window.location.reload()
}

async function deleteProductButton(productID, productName) {
    let doDelete = confirm(`Är du säker på att du vill ta bort ${productName} från produktlistan?`)
    if (doDelete) {
        await fetch(`/api/v1/products/${productID}`, {method: "DELETE"})
    }
    window.location.reload()
}

async function fetchData() {
    await fetch("/api/v1/products")
        .then((response) => response.text())
        .then((result) => {
        const PRODUCTS_OBJ = JSON.parse(result);
        PRODUCTS_OBJ.forEach(product => allProducts.push(product));
        })
        .catch((error) => console.error(error));
}

function waitForData() {
  if (allProducts.length === 0) {
    setTimeout(waitForData, 10);
  } else {
    populateTable()
  }
}

function populateTable() {
    const tableContent = document.getElementById("table-content")

    const AMOUNT_OF_COLUMNS = 6 // The total amount of columns on the table

    if (allProducts && allProducts.length > 0) {
        allProducts.forEach(product => {
            let productRow = document.createElement('TR')

            for (let i = 0; i < AMOUNT_OF_COLUMNS; i++) {
                const productDataCell = document.createElement('TD');
                let cellContent;
                switch(i) {
                    case 0: cellContent = product.id; break;
                    case 1: cellContent = product.category; break;
                    case 2: cellContent = product.name; break;
                    case 3: cellContent = product.price.toLocaleString("sv-SE") + " kr"; break;
                    case 4: cellContent = product.stock; break;
                    case 5: 
                        cellContent = document.createElement('form')
                        cellContent.action = `/api/v1/products/${product.id}`
                        cellContent.method = "DELETE"
                        let delBtn = document.createElement('button')
                        delBtn.type = "submit"
                        delBtn.addEventListener("click", () => deleteProductButton(product.id, product.name))
                        delBtn.textContent = "Ta bort"
                        cellContent.appendChild(delBtn)
                        break
                }

                if (typeof(cellContent) == "string" || typeof(cellContent) == "number") {
                    productDataCell.appendChild(document.createTextNode(cellContent));
                } else {
                    productDataCell.appendChild(cellContent)
                }

                productRow.appendChild(productDataCell);
            }
            tableContent.appendChild(productRow)
        })
    }
}

