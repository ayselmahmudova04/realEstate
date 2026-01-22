import React from 'react'
import search from '../images/search.svg'
import '../css/Search.css'

function Search() {
    return (
        <>
            <div className="search-all">
                <div className="search">
                    <div className="pwp-all">

                        <div className="pwp">
                            <h3>Yer</h3>
                            <input type="text" placeholder='Gediləcək yer axtarın' />
                        </div>
                    </div>
                    <div className="pwp-all">
                        <div className="pwp">
                            <h3>Nə zaman?</h3>
                            <input type="text" placeholder='Tarix əlavə edin' />
                        </div>
                    </div>
                    <div className="pwp-all">

                        <div className="pwp">
                            <h3>Adamlar</h3>
                            <input type="text" placeholder='Qonaq əlavə edin' />
                        </div>
                    </div>

                    <div className="search-ikon">
                        <img src={search} alt="" />
                    </div>
                </div>
            </div>

        </>
    )
}

export default Search