const data = [
    { id : 1, name : "Marlboro Red (20-pack)", categoryID : 1, price : 89.00, stock : 100},
    { id : 2, name : "Camel Blue (20-pack)", categoryID : 1, price : 85.00, stock : 100 },
    { id : 3, name : "L&M Filter (20-pack)", categoryID : 1, price : 79.00, stock : 100 },
    { id : 4, name : "Skruf Original Portion", categoryID : 1, price : 62.00, stock : 100 },
    { id : 5, name : "Göteborgs Rapé White Portion", categoryID : 1, price : 67.00, stock : 100 },

    { id : 6, name : "Marabou Mjölkchoklad 100 g", categoryID : 2, price : 25.00, stock : 100 },
    { id : 7, name : "Daim dubbel", categoryID : 2, price : 15.00, stock : 100 },
    { id : 8, name : "Kexchoklad", categoryID : 2, price : 12.00, stock : 100 },
    { id : 9, name : "Malaco Gott & Blandat 160 g", categoryID : 2, price : 28.00, stock : 100 },

    { id : 10, name : "Korv med bröd", categoryID : 3, price : 25.00, stock : 100 },
    { id : 11, name : "Varm toast (ost & skinka)", categoryID : 3, price : 30.00, stock : 100 },
    { id : 12, name : "Pirog (köttfärs)", categoryID : 3, price : 22.00, stock : 100 },
    { id : 13, name : "Färdig sallad (kyckling)", categoryID : 3, price : 49.00, stock : 100 },
    { id : 14, name : "Panini (mozzarella & pesto)", categoryID : 3, price : 45.00, stock : 100 },

    { id : 15, name : "Aftonbladet (dagens)", categoryID : 4, price : 28.00, stock : 100 },
    { id : 16, name : "Expressen (dagens)", categoryID : 4, price : 28.00, stock : 100 },
    { id : 17, name : "Illustrerad Vetenskap", categoryID : 4, price : 79.00, stock : 100 },
    { id : 18, name : "Kalle Anka & Co", categoryID : 4, price : 45.00, stock : 100 },
    { id : 19, name : "Allt om Mat", categoryID : 4, price : 69.00, stock : 100 },
]

document.addEventListener("DOMContentLoaded", () => {
    fetchData()
    populateTable()
})

function deleteProductButton(productID, productName) {
    let doDelete = confirm(`Är du säker på att du vill ta bort ${productName} från produktlistan?`)
    if (doDelete) {
        fetch(`/api/v1/products/${productID}`, {method: "DELETE"})
    }
}

async function fetchData() {
    let data = await fetch("/api/v1/products")
    let formattedData = await data.text()
    console.log(formattedData)
}

function populateTable() {
    const tableContent = document.getElementById("table-content")

    const AMOUNT_OF_COLUMNS = 7

    if (data && data.length > 0) {
        data.forEach(product => {
            let productRow = document.createElement('TR')

            for (let j = 0; j < AMOUNT_OF_COLUMNS; j++) {
                const productDataCell = document.createElement('TD');
                let cellContent;
                switch(j) {
                    case 0: cellContent = product.id; break;
                    case 1: cellContent = product.categoryID; break;
                    case 2: cellContent = product.name; break;
                    case 3: cellContent = product.price.toLocaleString("sv-SE") + " kr"; break;
                    case 4: cellContent = product.stock; break;
                    case 5: 
                        cellContent = document.createElement('button') 
                        cellContent.classList.add('editBtn')
                        cellContent.setAttribute('data-id', product.id)
                        cellContent.textContent = "Redigera"
                        break
                    case 6: 
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

