# Product Inventory API v2

## Overview

The Product Inventory API provides endpoints to manage products and track how many units of each product are in stock.
 It supports creating, updating, deleting products, and modifying stock quantities.

**Base URL:**  
/api/v2

**Content Type:**  
application/json


**Product Model:**  
```json
Product Object
{
  "id": ID,
  "name": "string",
  "category": (foreign key as in frontend),
  "price": "float",
  "stock": 0,
  "createdAt": "ISO-8601 timestamp",
  "updatedAt": "ISO-8601 timestamp"
}
```

Field|Type|Description
-----|-----|-----
id|integer|Unique product identifier
name|string|Product name
category|integer|Product category id
price|float|Price of the product
stock|integer|Number of items in stock 
createdAt|string|Time product was created
updatedAt|string|Time product was last updated

**Category Model:**
```json
Cateogry Object
{
  "id": ID,
  "name": "string",
  "color": "string",
  "createdAt": "ISO-8601 timestamp",
  "updatedAt": "ISO-8601 timestamp"
}
```

Field|Type|Description
-----|-----|-----
id|integer|Unique category identifier
name|string|Category name
color|string|Category color in hex
createdAt|string|Time category was created
updatedAt|string|Time category was last updated

## Endpoints

### 1. Create a Product
POST  
/products

**Request Body:**
```json
{
  "name": "Wireless Mouse",
  "category": 2,
  "price": 40,
  "stock": 50
}
```

**Response (201 Created):**
```json
{
  "id": ID,
  "name": "Wireless Mouse",
  "category": 2,
  "price": 40,
  "stock": 50,
  "createdAt": "2025-12-16T10:15:30Z",
  "updatedAt": "2025-12-16T10:15:30Z"
}
```


### 2. Get All Products
GET  
/products

**Response (200 OK):**
```json
[
  {
    "id": ID,
    "name": "Wireless Mouse",
    "price": 40,
    "category": 2,
    "stock": 50
  }
]
```


### 3. Get a Product by ID
GET  
/products/{productId}

**Response (200 OK):**
```json
{
  "id": ID,
  "name": "Wireless Mouse",
  "category": 2,
  "price": 40,
  "stock": 50
}
```


### 4. Update Product Details
PUT  
/products/{productId}

**Request Body:**
```json
{
  "name": "Wireless Mouse Pro",
  "category": 3
  "price": 40,
  "stock": 50
}
```

**Response (200 OK):**
```json
{
  "id": ID,
  "name": "Wireless Mouse Pro",
  "category": 3
  "price": 40,
  "stock": 50
}
```


### 5. Delete a Product
DELETE  
/products/{productId}

**Response (204 No Content):**

### 6. Increase Stock
POST  
/products/{productId}/stock/add

**Request Body:**
```json
{
  "amount": 10
}
```

**Response (200 OK):**
```json
{
  "id": ID,
  "stock": 60
}
```


### 7. Decrease Stock
POST  
/products/{productId}/stock/remove

**Request Body:**
```json
{
  "amount": 5
}
```

**Response (200 OK):**
```json
{
  "id": ID,
  "stock": 55
}
```


### 8. Create a Category
POST  
/categories

**Request Body:**
```json
{
  "name": "Electronics",
  "color": "1010fa"
}
```

**Response (201 Created):**
```json
{
  "id": ID,
  "name": "Electronics",
  "createdAt": "2025-12-16T10:15:30Z",
  "updatedAt": "2025-12-16T10:15:30Z"
}
```


### 9. Get All Categories
GET  
/categories

**Response (200 OK):**
```json
[
  {
    "id": ID,
    "name": "Electronics",
    "color": "1010fa"
  }
]
```


### 10. Get a Category by ID
GET  
/categories/{categoryId}

**Response (200 OK):**
```json
{
  "id": ID,
  "name": "Electronics",
  "color": "1010fa"
}
```


### 11. Update Category Details
PUT  
/categories/{categoryId}

**Request Body:**
```json
{
  "name": "Gaming",
  "color": "ff0000"
}
```

**Response (200 OK):**
```json
{
  "id": ID,
  "name": "Gaming",
  "color": "ff0000"
}
```


### 12. Delete a Category
DELETE  
/categories/{categoryId}

**Response (204 No Content):**


[Go back to README](../../README.md)
