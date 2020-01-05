import React, { createContext, useState } from 'react';
import styled from 'styled-components';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { DefaultLink, DefaultInput } from '../../theme/StyledComponents';

const MasterPanel = styled.div`
    display: flex;
    flex-direction: column;
    height: 100%;
`;

const HeaderBar = styled.div`
    display: flex;
`;

const SearchBar = styled(DefaultInput)`
    height: 30px;
    width: 200px;
`;

const CreateLink = styled(DefaultLink)`
    color: white;
    margin: auto;
`;

const CreateText = styled.div`
    border-radius: 5px;
    color: white;
    background-color: green;
    margin-left: auto;
    border: none;
    display: flex;
    padding: 0 10px 0 10px;
`;

const CreateIcon = styled(FontAwesomeIcon)`
    margin-right: 5px;
`;

const Content = styled.div`
    margin-top: 10px;
    overflow: hidden;
    display: flex;
`;

export const SearchContext = createContext('');

export const MasterView: React.FC<{
    createText: string;
    createUrl: string;
}> = ({ children, createText, createUrl }) => {
    const [searchterm, setSearchterm] = useState('');
    console.log('searchterm', searchterm);
    return (
        <MasterPanel>
            <HeaderBar>
                <SearchBar
                    value={searchterm}
                    onChange={e => setSearchterm(e.target.value)}
                    placeholder="Suche..."
                />
                <CreateText>
                    <CreateLink to={createUrl}>
                        <CreateIcon icon={faPlus} />
                        {createText}
                    </CreateLink>
                </CreateText>
            </HeaderBar>
            <Content>
                <SearchContext.Provider value={searchterm}>
                    {children}
                </SearchContext.Provider>
            </Content>
        </MasterPanel>
    );
};
