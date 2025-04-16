import React, { useContext, useEffect, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import InputField from '../../components/InputField'
import { UserContext } from '../../context/UserContext'

const UserPage = () => {
	const { getUser, updateUser, deleteUser } = useContext(UserContext)
	const { id } = useParams()
	const navigate = useNavigate()

	const [user, setUser] = useState(null)
	const [updateData, setUpdateData] = useState({
		name: '',
		email: '',
		password: '',
		role: '',
	})

	useEffect(() => {
		getUser(id)
			.then(res => {
				setUser(res.data)
				setUpdateData({
					name: res.data.name,
					email: res.data.email,
					password: '',
					role: res.data.role,
				})
			})
			.catch(err => {
				console.error(err.message)
			})
	}, [id, getUser])

	const handleUpdateUser = () => {
		const { name, email, password, role } = updateData
		if (!name || !email || !role) {
			return console.error('All fields are required for update')
		}
		const data = { name, email, password, role }
		updateUser(id, data)
			.then(res => {
				console.log('User updated:', res.data)
				setUser(res.data)
				setUpdateData({
					name: res.data.name,
					email: res.data.email,
					password: '',
					role: res.data.role,
				})
			})
			.catch(err => console.error(err.message))

		navigate(`/users/${id}`)
	}

	const handleDeleteUser = () => {
		deleteUser(id)
			.then(() => {
				navigate('/users')
			})
			.catch(err => console.error(err.message))
	}

	if (!user) {
		return <div>Loading user details...</div>
	}

	return (
		<div className='page user-page'>
			<h1 className='page__title'>User Details</h1>
			<p>Name: {user.name}</p>
			<p>Email: {user.email}</p>
			<p>Role: {user.role}</p>

			<InputField
				type='text'
				placeholder='Name'
				value={updateData.name}
				onChange={e => setUpdateData({ ...updateData, name: e.target.value })}
			/>
			<InputField
				type='email'
				placeholder='Email'
				value={updateData.email}
				onChange={e => setUpdateData({ ...updateData, email: e.target.value })}
			/>
			<InputField
				type='password'
				placeholder='Password'
				value={updateData.password}
				onChange={e =>
					setUpdateData({ ...updateData, password: e.target.value })
				}
			/>
			<InputField
				type='text'
				placeholder='Role'
				value={updateData.role}
				onChange={e => setUpdateData({ ...updateData, role: e.target.value })}
			/>
			<button
				className='user-page__button yellow-btn'
				onClick={handleUpdateUser}
			>
				Update User
			</button>
			<button className='user-page__button red-btn' onClick={handleDeleteUser}>
				Delete User
			</button>
		</div>
	)
}

export default UserPage
