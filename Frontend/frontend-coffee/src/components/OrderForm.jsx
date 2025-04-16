// src/components/OrderForm.jsx
import React, { useContext, useEffect, useState } from 'react'
import { ProductContext } from '../context/ProductContext'
import { UserContext } from '../context/UserContext'
import '../styles/orders/OrderForm.css'

const OrderForm = ({ onSave, initialData }) => {
	const [form, setForm] = useState({
		customerName: '',
		productId: '',
		quantity: 1,
	})

	const { token, loggedUser } = useContext(UserContext)
	const { products, fetchAllProducts } = useContext(ProductContext)

	useEffect(() => {
		if (token) fetchAllProducts()
	}, [token])

	useEffect(() => {
		if (initialData) setForm(initialData)
	}, [initialData])

	const handleChange = e => {
		const { name, value } = e.target
		setForm(prev => ({ ...prev, [name]: value }))
	}

	const handleSubmit = async e => {
		e.preventDefault()

		const productId = parseInt(form.productId, 10)
		const quantity = parseInt(form.quantity, 10)

		if (!productId || isNaN(productId)) {
			alert('Please select a valid product.')
			return
		}

		if (!quantity || quantity < 1) {
			alert('Quantity must be at least 1.')
			return
		}

		const result = {
			userId: loggedUser.id,
			orderProducts: [
				{
					customerName: loggedUser.name,
					customerEmail: loggedUser.email,
					productId,
					quantity,
				},
			],
		}

		await onSave(result)
		setForm({ customerName: '', productId: '', quantity: 1 })
	}

	return (
		<form className='order-form' onSubmit={handleSubmit}>
			<h2>{initialData ? 'Edit Order' : 'Create Order'}</h2>
			<select
				name='productId'
				value={form.productId}
				onChange={handleChange}
				required
			>
				<option hidden disabled value=''>
					Select Product
				</option>
				{products.map(product => (
					<option key={product.id} value={product.id}>
						{product.name} - ${product.price} - {product.description}
					</option>
				))}
			</select>
			<input
				type='number'
				name='quantity'
				value={form.quantity}
				min={1}
				onChange={handleChange}
				required
			/>
			<button type='submit'>{initialData ? 'Update' : 'Add'}</button>
		</form>
	)
}

export default OrderForm
