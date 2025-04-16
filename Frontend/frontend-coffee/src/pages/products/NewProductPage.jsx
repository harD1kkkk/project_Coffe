import React, { useContext, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import InputField from '../../components/InputField'
import { ProductContext } from '../../context/ProductContext'
import '../../styles/products/NewProductPage.css'

const NewProductPage = () => {
	const { createNewProduct } = useContext(ProductContext)
	const navigate = useNavigate()
	const [isCreating, setIsCreating] = useState(false)

	const [formData, setFormData] = useState({
		name: '',
		description: '',
		price: '',
	})

	const handleSubmit = async e => {
		e.preventDefault()
		setIsCreating(true)
		const { name, description, price, image, music } = formData

		if (!name || !description || !price) {
			setIsCreating(false)
			return
		}

		try {
			await createNewProduct(
				{ name, description, price: parseFloat(price) },
				image,
				music
			)
			navigate('/products')
		} catch (err) {
			console.error(err.message)
		} finally {
			setIsCreating(false)
		}
	}

	return (
		<div className='new-product-page'>
			<h1>Add New Product</h1>

			{isCreating && <p>Creating product...</p>}

			{!isCreating && (
				<div className='new-product-form'>
					<InputField
						type='text'
						name='name'
						placeholder='Product name'
						value={formData.name}
						onChange={e => setFormData({ ...formData, name: e.target.value })}
					/>
					<InputField
						type='text'
						name='description'
						placeholder='Product description'
						value={formData.description}
						onChange={e =>
							setFormData({ ...formData, description: e.target.value })
						}
					/>
					<InputField
						type='number'
						name='price'
						placeholder='Price'
						value={formData.price}
						onChange={e => setFormData({ ...formData, price: e.target.value })}
					/>
					<label htmlFor='image'>Image:</label>
					<InputField
						type='file'
						name='image'
						accept='image/*'
						onChange={e =>
							setFormData({ ...formData, image: e.target.files[0] })
						}
					/>
					<label htmlFor='music'>Audio File:</label>
					<InputField
						type='file'
						name='music'
						accept='audio/*'
						onChange={e =>
							setFormData({ ...formData, music: e.target.files[0] })
						}
					/>
					<button type='submit' className='submit-btn' onClick={handleSubmit}>
						Create Product
					</button>
				</div>
			)}
		</div>
	)
}

export default NewProductPage
