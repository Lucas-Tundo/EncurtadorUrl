document.getElementById('btnSubmit')
    .addEventListener('click', function (e) {
        e.preventDefault();

        handleSubmitAsync();
    });

document.getElementById('url')
    .addEventListener('keyup', function (evt) {
        if (evt.code === 'Enter') {
            event.preventDefault();
            handleSubmitAsync();
        }
    });

function handleSubmitAsync() {
    const url = document.getElementById('url').value;

    var json = { 'url': url };

    const headers = {
        'content-type': 'application/json'
    };

    fetch('/urls', { method: 'post', body: JSON.stringify(json), headers: headers })
        //.then(apiResult => apiResult.json())
        .then(apiResult => {
            return apiResult.json().then(data => {
                return {
                    ok: apiResult.ok,
                    status: apiResult.status,
                    json: data
                };
            });
        })
        .then(({ json, ok }) => {
            console.log(json);

            if (ok && json.shortUrl) {
                const anchor = `<a href="${json.shortUrl}" target="_blank">${json.shortUrl}</a>`;
                document.getElementById('urlResult').innerHTML = anchor;
            } else {
                alert(json.errorMessage || 'Erro ao encurtar URL');
            }
        });

        
}