import React from 'react'
import '../styles/users/UserList.css'
import UserListItem from './UserListItem.jsx'

const UserList = ({ users, onDelete }) => {
	return (
		<div className='user-list'>
			{users.map(user => (
				<UserListItem key={user.id} user={user} onDelete={onDelete} />
			))}
		</div>
	)
}

export default UserList
