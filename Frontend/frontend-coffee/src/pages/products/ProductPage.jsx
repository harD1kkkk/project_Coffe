import React, { useContext, useEffect, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import InputField from '../../components/InputField'
import { ProductContext } from '../../context/ProductContext'
import '../../styles/products/ProductPage.css'

const ProductPage = () => {
	const { id } = useParams()
	const navigate = useNavigate()

	const {
		getProduct,
		updateExistingProduct,
		updateProductImageFile,
		updateProductMusicFile,
		removeProduct,
	} = useContext(ProductContext)

	const [product, setProduct] = useState(null)
	const [formData, setFormData] = useState({
		name: '',
		description: '',
		price: 0,
		stock: 0,
	})
	const [imageFile, setImageFile] = useState(null)
	const [musicFile, setMusicFile] = useState(null)

	useEffect(() => {
		getProduct(id)
			.then(res => {
				setProduct(res.data)
				setFormData({
					name: res.data.name,
					description: res.data.description,
					price: res.data.price,
				})
			})
			.catch(err => {
				console.error(err.message)
			})
	}, [id])

	const handleUpdate = () => {
		updateExistingProduct(id, formData)
			.then(res => {
				setProduct(res.data)
			})
			.catch(err => console.error(err.message))
		navigate(`/products`)
	}

	const handleImageUpload = () => {
		if (imageFile) {
			updateProductImageFile(id, imageFile).catch(err =>
				console.error(err.message)
			)
		}
	}

	const handleMusicUpload = () => {
		if (musicFile) {
			updateProductMusicFile(id, musicFile).catch(err =>
				console.error(err.message)
			)
		}
	}

	const handleDelete = () => {
		removeProduct(id)
			.then(() => {
				navigate('/products')
			})
			.catch(err => console.error(err.message))
	}

	if (!product) return <div>Loading...</div>

	return (
		<div className='product-page'>
			<h1>Edit Product</h1>

			<InputField
				type='text'
				placeholder='Name'
				value={formData.name}
				onChange={e => setFormData({ ...formData, name: e.target.value })}
			/>
			<InputField
				type='text'
				placeholder='Description'
				value={formData.description}
				onChange={e =>
					setFormData({ ...formData, description: e.target.value })
				}
			/>
			<InputField
				type='number'
				placeholder='Price'
				value={formData.price}
				onChange={e => setFormData({ ...formData, price: e.target.value })}
			/>
			<InputField
				type='number'
				placeholder='Stock'
				value={formData.stock}
				onChange={e => setFormData({ ...formData, stock: e.target.value })}
			/>

			<input type='file' onChange={e => setImageFile(e.target.files[0])} />
			<button className='blue-btn' onClick={handleImageUpload}>
				Upload Image
			</button>

			<input type='file' onChange={e => setMusicFile(e.target.files[0])} />
			<button className='blue-btn' onClick={handleMusicUpload}>
				Upload Music
			</button>

			<button className='yellow-btn' onClick={handleUpdate}>
				Update Product
			</button>
			<button className='red-btn' onClick={handleDelete}>
				Delete Product
			</button>
		</div>
	)
}

export default ProductPage
