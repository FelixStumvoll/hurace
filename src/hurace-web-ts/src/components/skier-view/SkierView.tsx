import React, { useEffect, useState } from 'react';
import styled from 'styled-components';
import { Skier } from '../../interfaces/Skier';
import { setStateAsync } from '../../common/stateSetter';
import { getSkiers } from '../../api';
import { SkierListItem } from './SkierListItem';

const SkierPanel = styled.div`
    display: flex;
    flex-direction: column;
    width: 100%;
    height: 100%;
    overflow: hidden;
`;

const SearchBarWrapper = styled.div`
    display: flex;
`;

const SearchBar = styled.input`
    margin-left: auto;
    border-radius: 15px;
    padding-left: 10px;
    border: 1px solid ${props => props.theme.gray};
    height: 30px;
    width: 200px;
`;

const SkierList = styled.div`
    display: flex;
    flex-wrap: wrap;
    justify-content: center;
    margin-top: 10px;
    overflow: auto;
`;

const skierSearchFunc = (skier: Skier, searchTerm: string): boolean =>
    skier.firstName.includes(searchTerm) ||
    skier.lastName.includes(searchTerm) ||
    skier.country.countryName.includes(searchTerm);

export const SkierView: React.FC = () => {
    const [skiers, setSkiers] = useState<Skier[] | undefined>(undefined);
    const [searchTerm, setSearchTerm] = useState('');

    useEffect(() => {
        if (skiers !== undefined) return;
        setStateAsync(setSkiers, getSkiers());
    }, [skiers]);

    return (
        <SkierPanel>
            <SearchBarWrapper>
                <SearchBar
                    value={searchTerm}
                    onChange={e => setSearchTerm(e.target.value)}
                    placeholder="Suche"
                />
            </SearchBarWrapper>
            <SkierList>
                {skiers
                    ?.filter(s => skierSearchFunc(s, searchTerm))
                    .map(s => (
                        <SkierListItem key={s.id} skier={s} />
                    ))}
            </SkierList>
        </SkierPanel>
    );
};
