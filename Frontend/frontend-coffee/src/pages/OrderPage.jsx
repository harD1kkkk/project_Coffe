// src/pages/OrderPage.jsx
import React, { useContext, useEffect, useState } from 'react'
import OrderForm from '../components/OrderForm'
import OrderList from '../components/OrderList'
import { OrderContext } from '../context/OrderContext'
import { UserContext } from '../context/UserContext'
import '../styles/orders/OrderPage.css'

const OrderPage = () => {
	const { token } = useContext(UserContext)
	const {
		orders,
		fetchAllOrders,
		createNewOrder,
		updateExistingOrder,
		removeOrder,
		getOrder,
	} = useContext(OrderContext)

	const [editingOrder, setEditingOrder] = useState(null)

	useEffect(() => {
		if (token) fetchAllOrders()
	}, [token])

	const handleEdit = async id => {
		const res = await getOrder(id)
		setEditingOrder(res.data)
	}

	const handleDelete = async id => {
		await removeOrder(id)
	}

	const handleSave = async orderData => {
		if (editingOrder) {
			await updateExistingOrder(editingOrder.id, orderData)
		} else {
			await createNewOrder(orderData)
		}
		setEditingOrder(null)
	}

	return (
		<div className='page'>
			<h1 className='page__title'>Order Management</h1>
			{!token ? (
				<p>Please log in to manage orders.</p>
			) : (
				<>
					<OrderForm onSave={handleSave} initialData={editingOrder} />
					<OrderList
						orders={orders}
						onDelete={handleDelete}
						onEdit={handleEdit}
					/>
				</>
			)}
		</div>
	)
}

export default OrderPage
