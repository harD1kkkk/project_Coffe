import { useContext, useEffect } from 'react'
import UserList from '../../components/UserList.jsx'
import { UserContext } from '../../context/UserContext'
import '../../styles/users/UsersPage.css'

const UsersPage = () => {
	const { users, token, fetchAllUsers, deleteUser } = useContext(UserContext)

	useEffect(() => {
		if (!token) {
			console.error('Token is missing. Please log in.')
			return
		}
		fetchAllUsers().catch(err => console.error(err.message))
	}, [token])

	const handleDeleteUser = userId => {
		deleteUser(userId)
			.then(() => console.log('User deleted'))
			.catch(err => console.error(err.message))
	}

	return (
		<div className='page'>
			<h1 className='page__title'>Users Management</h1>

			{!token ? (
				<p>Please log in to manage users.</p>
			) : (
				<>
					<UserList users={users} onDelete={handleDeleteUser} />
				</>
			)}
		</div>
	)
}

export default UsersPage
