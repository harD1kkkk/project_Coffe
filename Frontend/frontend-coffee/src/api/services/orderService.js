import { ordersApi } from '../api'

const getAuthHeaders = token => ({
	headers: {
		Authorization: `Bearer ${token}`,
	},
})

export const getAllOrders = token => {
	return ordersApi.get('/', getAuthHeaders(token))
}

export const getOrderById = (id, token) => {
	return ordersApi.get(`/${id}`, getAuthHeaders(token))
}

export const createOrder = (orderDto, token) => {
	return ordersApi.post('/', orderDto, getAuthHeaders(token))
}

export const updateOrder = (id, orderDto, token) => {
	return ordersApi.put(`?id=${id}`, orderDto, getAuthHeaders(token))
}

export const deleteOrder = (id, token) => {
	return ordersApi.delete(`/${id}`, getAuthHeaders(token))
}
