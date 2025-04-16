import React, { createContext, useEffect, useState } from 'react'
import {
	deleteUser as apiDeleteUser,
	updateUser as apiUpdateUser,
	getAllUsers,
	getUserById,
} from '../api/services/userService'

export const UserContext = createContext()

export const UserProvider = ({ children }) => {
	const [users, setUsers] = useState([])
	const [token, setToken] = useState('')
	const [loggedUser, setLoggedUser] = useState(() => {
		const storedUser = localStorage.getItem('loggedUser')
		return storedUser
			? JSON.parse(storedUser)
			: { id: '', name: '', email: '', role: '' }
	})

	useEffect(() => {
		const storedToken = localStorage.getItem('token')
		if (storedToken) {
			setToken(storedToken)
		}
	}, [])

	const fetchAllUsers = async () => {
		if (!token) return Promise.reject(new Error('Token is required'))
		try {
			const res = await getAllUsers(token)
			setUsers(res.data)
			return res
		} catch (err) {
			console.error(err.response?.data || err.message)
			throw err
		}
	}

	const getUser = async id => {
		if (!token) return Promise.reject(new Error('Token is required'))
		try {
			const res = await getUserById(id, token)
			return res
		} catch (err) {
			console.error(err.response?.data || err.message)
			throw err
		}
	}

	const updateUser = async (id, updatedData) => {
		if (!token) return Promise.reject(new Error('Token is required'))
		try {
			const res = await apiUpdateUser(
				id,
				updatedData.name,
				updatedData.email,
				updatedData.password,
				updatedData.role,
				token
			)
			setUsers(prevUsers =>
				prevUsers.map(user => (user.id === id ? res.data : user))
			)
			return res
		} catch (err) {
			console.error(err.response?.data || err.message)
			throw err
		}
	}

	const deleteUser = async userId => {
		if (!token) return Promise.reject(new Error('Token is required'))
		try {
			await apiDeleteUser(userId, token)
			setUsers(prevUsers => prevUsers.filter(user => user.id !== userId))
		} catch (err) {
			console.error(err.response?.data || err.message)
			throw err
		}
	}

	return (
		<UserContext.Provider
			value={{
				users,
				token,
				setToken,
				loggedUser,
				setLoggedUser,
				fetchAllUsers,
				getUser,
				updateUser,
				deleteUser,
			}}
		>
			{children}
		</UserContext.Provider>
	)
}
