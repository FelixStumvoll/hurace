import React, { createContext, useState, useCallback } from 'react';
import styled from 'styled-components';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { DefaultLink, DefaultInput } from '../../theme/CustomComponents';
import { AuthContext } from '../Auth0Provider';

const MasterPanel = styled.div`
    display: flex;
    flex-direction: column;
    height: 100%;
`;

const HeaderBar = styled.div`
    display: flex;
    position: sticky;
    top: calc(${props => props.theme.navHeight} + 10px);
`;

const SearchBar = styled(DefaultInput)`
    height: 31px;
    width: 200px;
`;

const CreateLink = styled(DefaultLink)`
    color: white;

    border-radius: 5px;
    color: white;
    background-color: green;
    margin-left: auto;
    display: flex;
    padding: 0 10px 0 10px;
`;

const CreateText = styled.div`
    margin: auto;
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

export const ListViewWrapper: React.FC<{
    createText: string;
    createUrl: string;
}> = ({ children, createText, createUrl }) => {
    const [searchterm, setSearchterm] = useState('');

    const searchTermChange = useCallback(
        (event: React.ChangeEvent<HTMLInputElement>) => {
            setSearchterm(event.target.value);
        },
        [setSearchterm]
    );

    return (
        <MasterPanel>
            <HeaderBar>
                <SearchBar
                    value={searchterm}
                    onChange={searchTermChange}
                    placeholder="Suche..."
                />
                <AuthContext.Consumer>
                    {({ isAuthenticated }) =>
                        isAuthenticated && (
                            <CreateLink to={createUrl}>
                                <CreateText>
                                    <CreateIcon icon={faPlus} />
                                    {createText}
                                </CreateText>
                            </CreateLink>
                        )
                    }
                </AuthContext.Consumer>
            </HeaderBar>
            <Content>
                <SearchContext.Provider value={searchterm}>
                    {children}
                </SearchContext.Provider>
            </Content>
        </MasterPanel>
    );
};
