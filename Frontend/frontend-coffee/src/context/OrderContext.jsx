import React, { createContext, useContext, useState } from 'react'
import {
	createOrder,
	deleteOrder,
	getAllOrders,
	getOrderById,
	updateOrder,
} from '../api/services/orderService'
import { UserContext } from './UserContext'

export const OrderContext = createContext()

export const OrderProvider = ({ children }) => {
	const [orders, setOrders] = useState([])
	const { token } = useContext(UserContext)

	const fetchAllOrders = async () => {
		if (!token) return
		const res = await getAllOrders(token)
		setOrders(res.data)
	}

	const createNewOrder = async orderDto => {
		if (!token) return
		console.log('Adding new order:', orderDto)

		const res = await createOrder(orderDto, token)
		setOrders(prev => [...prev, res.data])
	}

	const updateExistingOrder = async (id, updatedDto) => {
		if (!token) return
		const res = await updateOrder(id, updatedDto, token)
		setOrders(prev => prev.map(o => (o.id === id ? res.data : o)))
	}

	const removeOrder = async id => {
		if (!token) return
		await deleteOrder(id, token)
		setOrders(prev => prev.filter(o => o.id !== id))
	}

	const getOrder = async id => {
		if (!token) return
		const res = await getOrderById(id, token)
		return res.data
	}

	return (
		<OrderContext.Provider
			value={{
				orders,
				fetchAllOrders,
				createNewOrder,
				updateExistingOrder,
				removeOrder,
				getOrder,
			}}
		>
			{children}
		</OrderContext.Provider>
	)
}
