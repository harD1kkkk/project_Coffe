import '../styles/InputField.css'

const InputField = ({ type, placeholder, value, onChange }) => (
	<div className='input-field'>
		<input
			className='input-field__input'
			type={type}
			placeholder={placeholder}
			value={value}
			onChange={onChange}
		/>
	</div>
)

export default InputField
