function HidePassword()
{
	const passwordFild = document.getElementById('form_field_password-input')

	icon0 = document.getElementById('form_field_password_icon0')
	icon1 = document.getElementById('form_field_password_icon1')

	if (passwordFild.type === 'password') {
		passwordFild.type = 'text'

		icon0.hidden = true;
		icon1.hidden = false;
	}
	else {
		passwordFild.type = 'password'

		icon0.hidden = false;
		icon1.hidden = true;
	}
}

async function Continue()
{
	const email = document.getElementById('form_field_email-input').value
	const password = document.getElementById('form_field_password-input').value

	const resultMasage = document.getElementById('form_result')

	const responce = await fetch('/api/auth/login',
	{
		method: 'POST',
		headers: { 'Content-Type': 'application/json' },
		body: JSON.stringify({ email, password })
	})

	if (responce.ok)
	{
		const { accessToken } = await responce.json()
		localStorage.setItem('accessToken', accessToken)

		resultMasage.textContent = '✅ access token:\n' + accessToken +'\n'
	}
	else
	{
		resultMasage.textContent = '❌ fail'
	}
}




document.addEventListener('DOMContentLoaded', () =>
{
	document.getElementById('csrfToken').value = crypto.randomUUID()
});