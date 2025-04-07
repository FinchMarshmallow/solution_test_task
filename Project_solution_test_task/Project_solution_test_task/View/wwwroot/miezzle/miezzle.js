function hidePassword() {
	const passwordFild = document.getElementById('form_field_password-input')
	const buttonFild = document.getElementById('form_field_password-button').value

	if (passwordFild.type === 'password') {
		passwordFild.type = 'text'
		/*buttonFild = 🙉*/
	}
	else {
		passwordFild.type = 'password'
		/*buttonFild = 🙈*/
	}
}

document.getElementById('form_continue-button').addEventListener('click', async function (e)
{
	e.preventDefault();

	const email = document.getElementById("form_field_email-input").value

	const password = document.getElementById("form_field_password-input").value

	//if (!email || !password) {
	//	alert('null');
	//	return;
	//}

	try
	{
		const response = await fetch('/api/login',
		{
			method: 'POST',
			headers:
			{
				'Content-Type': 'application/json',
				'X-CSRF-TOKEN': document.getElementById('csrfToken').value
			},
			body: JSON.stringify({ email, password })
		});

		const result = await response.json();

		if (!response.ok) throw new Error(result.message);

		alert('good input');

	}
	catch (error)
	{
		console.error('error json:', error);
		alert(error.message);
	}
})

document.addEventListener('DOMContentLoaded', () =>
{
	document.getElementById('csrfToken').value = crypto.randomUUID();
});