import { useContext } from 'react'
import { Link, useNavigate } from 'react-router'
import { UserContext } from '../context/UserContext'
import '../styles/Header.css'
import { showAlert } from '../utils/functions.js'

const Header = () => {
	const { token } = useContext(UserContext)
	const navigate = useNavigate()

	const handleLogout = () => {
		localStorage.removeItem('token')
		navigate('/auth')
		showAlert('Logged out successfully')
	}

	return (
		<header>
			<div className='header__logo'>
				<Link to='/'>
					<img
						src='/coffee-icon.png'
						alt='Coffee Image'
						width={40}
						height={40}
					/>
				</Link>
				{token && <h4>Coffee Shop</h4>}
			</div>

			<button className='logout-btn blue-btn' onClick={handleLogout}>
				Logout
			</button>

			<nav>
				<Link to='/'>Home</Link>
				{!token && <Link to='/auth'>Login/Register</Link>}
				<Link to='/users'>Users</Link>
				<Link to='/products'>Products</Link>
				<Link to='/orders'>Orders</Link>
			</nav>
		</header>
	)
}

export default Header
