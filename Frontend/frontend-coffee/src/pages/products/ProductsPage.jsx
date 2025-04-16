import React, { useContext, useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import InputField from '../../components/InputField'
import ProductList from '../../components/ProductList'
import { ProductContext } from '../../context/ProductContext'
import { UserContext } from '../../context/UserContext'
import '../../styles/products/ProductsPage.css'

const ProductsPage = () => {
	const { products, fetchAllProducts, removeProduct, searchProducts } =
		useContext(ProductContext)
	const { token } = useContext(UserContext)

	const [searchTerm, setSearchTerm] = useState('')
	const [sortBy, setSortBy] = useState('name')
	const navigate = useNavigate()

	useEffect(() => {
		if (token) fetchAllProducts()
	}, [token])

	const handleDeleteProduct = id => {
		removeProduct(id)
			.then(() => console.log('Product deleted'))
			.catch(err => console.error(err.message))
	}

	const handleSearch = () => {
		searchProducts({ searchTerm, sortBy })
			.then(() => console.log('Search done'))
			.catch(err => console.error(err.message))
	}

	const handleAddProduct = () => {
		navigate('/products/new')
	}

	return (
		<div className='page'>
			<h1 className='page__title'>Products Management</h1>

			{!token ? (
				<p>Please log in to manage users.</p>
			) : (
				<div className='top-bar'>
					<div className='search-bar'>
						<InputField
							type='text'
							placeholder='Search product...'
							value={searchTerm}
							onChange={e => setSearchTerm(e.target.value)}
						/>
						<select onChange={e => setSortBy(e.target.value)} value={sortBy}>
							<option value='name'>Name</option>
							<option value='low-price'>Low Price</option>
							<option value='high-price'>High Price</option>
						</select>
						<button className='blue-btn' onClick={handleSearch}>
							Search
						</button>
					</div>
					<button className='add-btn' onClick={handleAddProduct}>
						Add Product
					</button>
				</div>
			)}

			<ProductList products={products} onDelete={handleDeleteProduct} />
		</div>
	)
}

export default ProductsPage
