import React, { useContext, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { loginUser, registerUser } from '../../api/services/userService'
import InputField from '../../components/InputField.jsx'
import { UserContext } from '../../context/UserContext'
import '../../styles/AuthPage.css'
import { showAlert } from '../../utils/functions.js'

const AuthPage = () => {
	const { setToken, setLoggedUser } = useContext(UserContext)
	const navigate = useNavigate()

	const [isLogin, setIsLogin] = useState(true)
	const [formData, setFormData] = useState({
		name: '',
		email: '',
		password: '',
	})

	const handleSubmit = () => {
		const { name, email, password } = formData
		if (!email || !password || (!isLogin && !name)) {
			return console.error('All fields are required')
		}

		if (isLogin) {
			loginUser(email, password)
				.then(res => {
					const data = res.data
					localStorage.setItem('token', data.token)
					setToken(data.token)
					setLoggedUser(data.user.value)
					localStorage.setItem('loggedUser', JSON.stringify(data.user.value))
					navigate('/users')
					showAlert('Logged in successfully')
				})
				.catch(err => showAlert(err.response?.data || err.message))
		} else {
			registerUser(name, email, password)
				.then(res => {
					console.log('Registered:', res.data)
					setIsLogin(true)
					showAlert('Registered successfully')
				})
				.catch(err => showAlert(err.response?.data || err.message))
		}
	}

	return (
		<div className='auth-page'>
			<h1>{isLogin ? 'Login' : 'Register'}</h1>
			{!isLogin && (
				<InputField
					type='text'
					placeholder='Name'
					value={formData.name}
					onChange={e => setFormData({ ...formData, name: e.target.value })}
				/>
			)}
			<InputField
				type='email'
				placeholder='Email'
				value={formData.email}
				onChange={e => setFormData({ ...formData, email: e.target.value })}
			/>
			<InputField
				type='password'
				placeholder='Password'
				value={formData.password}
				onChange={e => setFormData({ ...formData, password: e.target.value })}
			/>
			<button className='auth-page__button' onClick={handleSubmit}>
				{isLogin ? 'Login' : 'Register'}
			</button>
			<p>
				{isLogin ? "Don't have an account?" : 'Already have an account?'}{' '}
				<span
					className='auth-page__toggle'
					onClick={() => setIsLogin(!isLogin)}
				>
					{isLogin ? 'Register' : 'Login'}
				</span>
			</p>
		</div>
	)
}

export default AuthPage
