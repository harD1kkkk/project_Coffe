import React from 'react'
import { Link } from 'react-router-dom'
import '../styles/products/ProductList.css'

const ProductList = ({ products, onDelete }) => {
	return (
		<div className='product-list'>
			{products.map(product => (
				<div key={product.id} className='product-item'>
					<img
						src={product.imagePath}
						alt={product.name}
						className='product-image'
						width={200}
						height={100}
					/>
					{product.musicPath && (
						<audio controls className='product-audio'>
							<source src={product.musicPath} type='audio/mpeg' />
							Your browser does not support the audio element.
						</audio>
					)}
					<h3>{product.name}</h3>
					<p>{product.description}</p>
					<p>Stock: {product.stock}</p>
					<p>Price: ${product.price}</p>
					<div className='actions'>
						<Link to={`/products/${product.id}`} className='yellow-btn'>
							Edit
						</Link>
						<button onClick={() => onDelete(product.id)} className='red-btn'>
							Delete
						</button>
					</div>
				</div>
			))}
		</div>
	)
}

export default ProductList
