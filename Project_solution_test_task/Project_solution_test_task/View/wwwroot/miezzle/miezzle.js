window.addEventListener('load', async () =>
{
	const tokenMassge = document.getElementById('tokens_view')

	const responce = await fetch("/api/auth/refresh")
	{
		method: 'GET'
	}

	if (responce.ok)
	{
		const { accessToken } = await responce.json()
		localStorage.setItem('accessToken', accessToken)
		tokenMassge.textContent = accessToken
	}
});

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
	const selectLogin = document.getElementById("select_type_login")

	const email = document.getElementById('form_field_email-input').value
	const password = document.getElementById('form_field_password-input').value

	const resultFild = document.getElementById('form_result')
	const tokenFild = document.getElementById('tokens_view')

	if (selectLogin.value === 'sign in')
	{
		SignIn(email, password, resultFild, tokenFild)
	}
	else
	{
		SingUp(email, password, resultFild, tokenFild, selectLogin)
	}
}

async function SignIn(email, password, resultFild, tokenFild)
{
	role = ''

	const responce = await fetch('/api/auth/sign in',
	{
		method: 'POST',
		headers: { 'Content-Type': 'application/json' },
		body: JSON.stringify({ email, password, role})
	})

	if (responce.ok)
	{
		const { accessToken } = await responce.json()
		localStorage.setItem('accessToken', accessToken)

		resultFild.textContent = '✅ good login'
		tokenFild.textContent = accessToken
	}
	else
	{
		resultFild.textContent = '❌ fail'
	}
}

async function SingUp(email, password, resultFild, tokenFild, selectLogin)
{
	let role

	if (selectLogin.value === 'sign up user')
	{
		role = 'Default'
	}
	else if (selectLogin.value === 'sign up admin')
	{
		role = 'Admin'
	}
	else
	{
		resultFild.textContent = '❌ fail type user'
		return
	}

	const responce = await fetch('/api/auth/sign up',
	{
		method: 'POST',
		headers: { 'Content-Type': 'application/json' },
		body: JSON.stringify(
		{
			email, password, role
		})
	})

	if (responce.ok)
	{
		const { accessToken } = await responce.json()
		localStorage.setItem('accessToken', accessToken)

		resultFild.textContent = '✅ hello! ' + role
		tokenFild.textContent = accessToken
	}
	else
	{
		resultFild.textContent = '❌ fail sign' + role
	}
}



document.addEventListener('DOMContentLoaded', () =>
{
	document.getElementById('csrfToken').value = crypto.randomUUID()
});