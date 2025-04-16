// src/components/OrderList.jsx
import React from 'react'
import '../styles/orders/OrderList.css'

const OrderList = ({ orders, onEdit, onDelete }) => {
	console.log(orders)
	return (
		<div className='order-list'>
			<h2>Orders</h2>
			{orders.length === 0 ? (
				<p className='not-found'>No orders found.</p>
			) : (
				<ul>
					{orders.map(order => (
						<li key={order.id} className='order-item'>
							<div>
								<strong>{order.user.name}</strong> —{' '}
								<strong>{order.user.email}</strong> — {order.product} (
								{order.orderProducts[0].quantity})
							</div>
							<div className='actions'>
								<button onClick={() => onEdit(order.id)}>Edit</button>
								<button onClick={() => onDelete(order.id)} className='delete'>
									Delete
								</button>
							</div>
						</li>
					))}
				</ul>
			)}
		</div>
	)
}

export default OrderList
