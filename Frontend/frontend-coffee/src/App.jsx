import React from 'react'
import { Route, BrowserRouter as Router, Routes } from 'react-router-dom'
import Header from './components/Header'
import { OrderProvider } from './context/OrderContext'
import { ProductProvider } from './context/ProductContext'
import { UserProvider } from './context/UserContext'
import AuthPage from './pages/auth/AuthPage'
import HomePage from './pages/HomePage'
import OrderPage from './pages/OrderPage'
import NewProductPage from './pages/products/NewProductPage'
import ProductPage from './pages/products/ProductPage'
import ProductsPage from './pages/products/ProductsPage'
import UserPage from './pages/users/UserPage'
import UsersPage from './pages/users/UsersPage'

function App() {
	return (
		<UserProvider>
			<ProductProvider>
				<OrderProvider>
					<Router>
						<Header />
						<Routes>
							<Route path='/' element={<HomePage />} />
							<Route path='/auth' element={<AuthPage />} />
							<Route path='/users' element={<UsersPage />} />
							<Route path='/users/:id' element={<UserPage />} />
							<Route path='/products' element={<ProductsPage />} />
							<Route path='/products/:id' element={<ProductPage />} />
							<Route path='/products/new' element={<NewProductPage />} />
							<Route path='/orders' element={<OrderPage />} />
						</Routes>
					</Router>
				</OrderProvider>
			</ProductProvider>
		</UserProvider>
	)
}

export default App
