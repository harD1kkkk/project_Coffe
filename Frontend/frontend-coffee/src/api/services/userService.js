import { usersApi } from '../api'

const getAuthHeaders = token => ({
	headers: {
		Authorization: `Bearer ${token}`,
	},
})

export const registerUser = async (name, email, password) => {
	return usersApi.post(`/register`, null, {
		params: { name, email, password },
	})
}

export const loginUser = async (email, password) => {
	return usersApi.post(`/login`, null, {
		params: { email, password },
	})
}

export const getUserById = async (id, token) => {
	return usersApi.get(`/${id}`, getAuthHeaders(token))
}

export const getAllUsers = async token => {
	return usersApi.get(`/`, getAuthHeaders(token))
}

export const updateUser = async (id, name, email, password, role, token) => {
	return usersApi.put(`/${id}`, null, {
		params: { name, email, password, role },
		...getAuthHeaders(token),
	})
}

export const deleteUser = async (id, token) => {
	return usersApi.delete(`/${id}`, getAuthHeaders(token))
}
