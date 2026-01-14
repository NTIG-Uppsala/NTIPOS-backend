const addProductForm = document.getElementById("add-product-form")

const allProducts = []

const allCategories = []

document.addEventListener("DOMContentLoaded", () => {
    fetchData()
    waitForData()
})

addProductForm.onsubmit = function() {
    var formData = new FormData(document.getElementById("add-product-form"))
    formData = Object.fromEntries(formData)
    formData.stock = formData.stock == "" ? 0 : formData.stock
    fetch("/api/v2/products", {method: "POST", body: JSON.stringify(formData), headers: {"Content-Type": "application/json",}})
    window.location.reload()
}

async function deleteProductButton(productID, productName) {
    let doDelete = confirm(`Är du säker på att du vill ta bort ${productName} från produktlistan?`)
    if (doDelete) {
        await fetch(`/api/v2/products/${productID}`, {method: "DELETE"})
    }
    window.location.reload()
}

async function fetchData() {
    await fetch("/api/v2/products")
        .then((response) => response.text())
        .then((result) => {
        const PRODUCTS_OBJ = JSON.parse(result);
        PRODUCTS_OBJ.forEach(product => allProducts.push(product));
        })
        .catch((error) => console.error(error));

    await fetch("/api/v2/categories")
        .then((response) => response.text())
        .then((result) => {
        const CATEGORIES_OBJ = JSON.parse(result);
        CATEGORIES_OBJ.forEach(category => allCategories.push(category));
        })
        .catch((error) => console.error(error));
}

function waitForData() {
  if (allProducts.length === 0 || allCategories.length === 0) {
    setTimeout(waitForData, 10);
  } else {
    insertCategoryProperties()
    populateTable()
    populateCategories()
  }
}

function insertCategoryProperties() {
    allProducts.forEach(product => {
        let category = allCategories.find(value => { return value.id == product.category})
        product.category = category.name
        product.color = category.color
    })
}

function populateCategories() {
    const categorySelector = document.getElementById("categorySelector")

    if (allCategories && allCategories.length > 0) {
        allCategories.forEach(category => {
            let categoryOption = document.createElement('OPTION')
            categoryOption.value = category.id
            categoryOption.textContent = category.name

            categorySelector.appendChild(categoryOption)
        })
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
                    case 1: 
                        cellContent = document.createElement('DIV');
                        console.log(product.color)
                        cellContent.style.backgroundColor = `#${product.color}`
                        cellContent.style.margin = "-8px"
                        cellContent.style.padding = "8px"
                        cellContent.textContent = product.category; break;
                    case 2: cellContent = product.name; break;
                    case 3: cellContent = product.price.toLocaleString("sv-SE") + " kr"; break;
                    case 4: cellContent = product.stock; break;
                    case 5: 
                        cellContent = document.createElement('form')
                        cellContent.action = `/api/v2/products/${product.id}`
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

