import axios from 'axios'

const BASE_URL = 'http://localhost:5083/api'

export const usersApi = axios.create({
	baseURL: `${BASE_URL || 'http://localhost:5000/api'}/User`,
	headers: {
		'Content-Type': 'application/json',
	},
})

export const productsApi = axios.create({
	baseURL: `${BASE_URL || 'http://localhost:5000/api'}/Product`,
})

export const ordersApi = axios.create({
	baseURL: `${BASE_URL || 'http://localhost:5000/api'}/Order`,
	headers: {
		'Content-Type': 'application/json',
	},
})

export const coffeeRecomendationsApi = axios.create({
	baseURL: `${BASE_URL || 'http://localhost:5000/api'}/CoffeeRecomendation`,
	headers: {
		'Content-Type': 'application/json',
	},
})
